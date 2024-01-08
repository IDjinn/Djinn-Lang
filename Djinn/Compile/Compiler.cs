using Djinn.Compile.Scopes;
using Djinn.Compile.Types;
using Djinn.Syntax.Biding.Statements;
using LLVMSharp;

namespace Djinn.Compile;

public static class Compiler
{
    public static void Compile(IEnumerable<IBoundStatement> syntaxTree)
    {
        var globalScope = new CompilationScope("global");
        var ctx = new CompilationContext(
            globalScope,
            LLVM.ModuleCreateWithName("main"),
            LLVM.CreateBuilder(),
            LLVM.ContextCreate()
        );
        
        SetupLLVMUtils(ctx);
        foreach (var statement in syntaxTree)
        {
            CompileStatements.GenerateStatement(ctx, statement);
        }
        string moduleError = "";
        LLVM.VerifyModule(ctx.Module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out moduleError);
        Console.WriteLine(moduleError);

        string error = "";
        LLVM.DumpModule(ctx.Module);
        LLVM.PrintModuleToFile(ctx.Module, "test.ll", out error);
        Console.WriteLine(error);
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