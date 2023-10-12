using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax;
using Djinn.Utils;
using LLVMSharp;

namespace Djinn.Compile;

public class CodeGen : IStatementVisitor<object>, IExpressionVisitor<object>
{
    private static readonly LLVMBool LLVMBoolFalse = new(0);
    private static readonly LLVMBool LLVMBoolTrue = new(1);
    private readonly SyntaxTree _syntaxTree;


    public CodeGen(SyntaxTree syntaxTree)
    {
        _syntaxTree = syntaxTree;
        Statements = syntaxTree.Statements;
        Module = LLVM.ModuleCreateWithName("main");
        Builder = LLVM.CreateBuilder();
    }

    private LLVMModuleRef Module { get; }
    private LLVMBuilderRef Builder { get; }
    private IReadOnlyList<IStatement> Statements { get; }

    public object Visit(BinaryExpressionSyntax expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(ConstantNumberExpressionSyntax expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(ConstantStringExpressionSyntax expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(FunctionStatement functionStatement)
    {
        throw new NotImplementedException();
    }

    public object Visit(ReturnStatement returnStatement)
    {
        throw new NotImplementedException();
    }

    public LLVMModuleRef GenerateLlvm()
    {
        var stringType = LLVMTypeRef.PointerType(LLVMTypeRef.Int8Type(), 0);
        var printfArguments = new[] { stringType };
        var printf = LLVM.AddFunction(Module, "printf",
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), printfArguments, LLVMBoolTrue));
        LLVM.SetLinkage(printf, LLVMLinkage.LLVMExternalLinkage);

        var scanfArguments = new[] { stringType };
        var scanf = LLVM.AddFunction(Module, "scanf",
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), scanfArguments, LLVMBoolTrue));
        LLVM.SetLinkage(scanf, LLVMLinkage.LLVMExternalLinkage);

        // Generate functions, globals, etc.
        foreach (var statement in Statements)
        {
            statement.Generate(this);
        }

        // Generate everything inside the functions
        // while (_valueStack.Count > 0)
        // {
        //     _valueStack.Peek().Item2.Accept(this);
        // }

        return Module;
    }
}