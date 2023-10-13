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
    public readonly Stack<Tuple<LLVMValueRef, object>> Stack = new Stack<Tuple<LLVMValueRef, object>>();


    public CodeGen(SyntaxTree syntaxTree)
    {
        _syntaxTree = syntaxTree;
        Statements = syntaxTree.Statements;
        Module = LLVM.ModuleCreateWithName("main");
        Builder = LLVM.CreateBuilder();
    }

    public LLVMModuleRef Module { get; set; }
    public LLVMBuilderRef Builder { get; }
    public IReadOnlyList<IStatement> Statements { get; }

    public object Visit(BinaryExpressionSyntax expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(ConstantNumberExpressionSyntax expressionSyntax)
    {
        var value = Convert.ToUInt64(expressionSyntax.NumberToken.Value.ToString());
        return LLVM.ConstInt(LLVM.Int64Type(), value, LLVMBoolFalse);
    }

    public object Visit(ConstantStringExpressionSyntax expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(UnaryExpressionSyntax expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(ParameterExpression expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(NoOpExpression expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(NameExpression expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(FunctionCallExpression expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(ConstantBooleanExpression expressionSyntax)
    {
        throw new NotImplementedException();
    }

    public object Visit(AssigmentExpression expressionSyntax)
    {
        var value = expressionSyntax.Expression.Accept(this);

        return value;
    }

    public object Visit(ArgumentsExpression expressionSyntax)
    {
        foreach (var argument in expressionSyntax.Arguments)
        {
            argument.Accept(this);
        }

        return default;
    }

    public object Visit(DiscardExpressionResultStatement discardExpressionResult)
    {
        return discardExpressionResult.Expression.Accept(this);
    }

    public object Visit(FunctionStatement functionStatement)
    {
        throw new NotImplementedException();
    }

    public object Visit(ReturnStatement returnStatement)
    {
        throw new NotImplementedException();
    }

    public object Visit(BlockStatement blockStatement)
    {
        throw new NotImplementedException();
    }

    public object Visit(FunctionDeclarationStatement functionDeclarationStatement)
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
            statement.Generate(this, this);
        }

        // Generate everything inside the functions
        // while (_valueStack.Count > 0)
        // {
        //     _valueStack.Peek().Item2.Accept(this);
        // }

        return Module;
    }
}