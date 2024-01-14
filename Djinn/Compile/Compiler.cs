using System.Runtime.InteropServices;
using Djinn.Compile.Scopes;
using Djinn.Compile.Types;
using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;
using Djinn.Syntax.Biding.Statements;
using LLVMSharp;

namespace Djinn.Compile;

public static class Compiler
{
    public readonly record struct CompilerOptions(
        string OutputFileName
    );
    public readonly record struct CompilationResult(
        string? Ir,
        CompilationContext Context
    );
    
    public static CompilationResult Compile(string sourceCode, CompilerOptions options)
    {
        var lexer = new Lexer(sourceCode);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        if (parser.Diagnostics.Any())
        {
            throw new Exception();
        }

        var binder = new Binder();
        var syntaxTree = binder.Bind(tree);
        if (binder.Reporter.Diagnostics.Any())
        {
            throw new Exception();
        }
        
        // LLVM.InitializeAllTargets();
        LLVM.InitializeX86Target();
        LLVM.InitializeX86AsmParser();
        LLVM.InitializeX86TargetMC();
        LLVM.InitializeX86TargetInfo();
        LLVM.InitializeX86AsmPrinter();
        LLVM.InitializeX86AsmParser();
        
        var globalScope = new CompilationScope("global");
        var module = LLVM.ModuleCreateWithName("main");
        var builder = LLVM.CreateBuilder();
        var context = LLVM.ContextCreate();

        var engine = InitializeJIT(module);
        var ctx = new CompilationContext(
            globalScope,
            module,
            builder,
            context,
            engine
        );
        
        SetupLLVMUtils(ctx);
        foreach (var statement in syntaxTree)
        {
            CompileStatements.GenerateStatement(ctx, statement);
        }
        
        LLVM.VerifyModule(ctx.Module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error);
        Console.WriteLine(error);
        
        var moduleIrResult = LLVM.PrintModuleToString(ctx.Module);
        return new(Marshal.PtrToStringAnsi(moduleIrResult), ctx);
    }

    public static void WriteToFile(CompilationResult compilationResult, CompilerOptions options)
    {
        LLVM.DumpModule(compilationResult.Context.Module);
        LLVM.PrintModuleToFile(compilationResult.Context.Module, $"{options.OutputFileName}.ll", out var error);
        Console.WriteLine(error);
    }

    private static LLVMExecutionEngineRef InitializeJIT(LLVMModuleRef module)
    {
        LLVMExecutionEngineRef engine;
        LLVM.LinkInMCJIT();
        var options = new LLVMMCJITCompilerOptions();
        LLVM.InitializeMCJITCompilerOptions(options);
        LLVM.CreateMCJITCompilerForModule(out engine, module, options, out var error);
        
        if (error != null)
            throw new Exception($"Error creating JIT compiler: {error}");

        if (engine.Pointer == IntPtr.Zero)
            throw new Exception();

        return engine;
    }

    public static void SetupLLVMUtils(CompilationContext ctx)
    {
        var stringType = LLVMTypeRef.PointerType(LLVMTypeRef.Int8Type(), 0);
        var printfArguments = new[] { stringType };
        var printf = LLVM.AddFunction(ctx.Module, "printf",
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), printfArguments, new LLVMBool(1)));
        LLVM.SetLinkage(printf, LLVMLinkage.LLVMExternalLinkage);
        STD.Functions["printf"] = printf;

        var scanfArguments = new[] { stringType };
        var scanf = LLVM.AddFunction(ctx.Module, "scanf",
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), scanfArguments, new LLVMBool(0)));
        LLVM.SetLinkage(scanf, LLVMLinkage.LLVMExternalLinkage);
        STD.Functions["scanf"] = scanf;

        foreach (var (identifier, functionPointer) in STD.Functions)
        {
            ctx.Scope.TryCreateFunction(identifier,functionPointer);
        }
    }
    
    public static class STD
    {
        public static readonly IDictionary<string, CompilationType> Types = new Dictionary<string, CompilationType>
        {
            {"int32", new CompilationType()}
        };

        public static IDictionary<string, LLVMValueRef> Functions = new Dictionary<string, LLVMValueRef>
        {

        };
    }
}