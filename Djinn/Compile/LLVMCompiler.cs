using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;
using LLVMSharp;

namespace Djinn.Compile;

public class LLVMCompiler : IBoundExpressionVisitor, IBoundStatementGenerator
{
    public Stack<KeyValuePair<LLVMValueRef, LLVMValueRef>>
        Stack = new Stack<KeyValuePair<LLVMValueRef, LLVMValueRef>>();

    public LLVMCompiler()
    {
        Module = LLVM.ModuleCreateWithName("main");
        Builder = LLVM.CreateBuilder();
        Context = LLVM.ContextCreate();
    }

    public LLVMModuleRef Module { get; init; }
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }

    public LLVMValueRef Visit(IBoundExpression boundExpression)
    {
        return boundExpression switch
        {
            BoundLiteralExpression literal => Visit(literal),
            BoundUnaryExpression unary => Visit(unary),
            BoundBinaryExpression binary => Visit(binary),
            _ => throw new NotImplementedException()
        };
    }

    public LLVMValueRef Visit(BoundBinaryExpression boundBinaryExpression)
    {
        return boundBinaryExpression.Evaluate(this);
    }

    public LLVMValueRef Visit(BoundUnaryExpression boundUnaryExpression)
    {
        var value = boundUnaryExpression.Evaluate(this);
        return value;
    }

    public LLVMValueRef Visit(BoundLiteralExpression boundLiteralExpression)
    {
        var value = boundLiteralExpression.Evaluate(this);
        return value;
    }

    public LLVMValueRef Generate(BoundReturnStatement returnStatement)
    {
        var value = returnStatement.Expression.Evaluate(this);
        return LLVM.BuildRet(Builder, value);
    }

    public LLVMValueRef Generate(BoundBlockStatement blockStatement)
    {
        LLVMValueRef block = new LLVMValueRef();
        foreach (var boundStatement in blockStatement.Statements)
        {
            block = Generate(boundStatement);
        }

        return block;
    }

    public LLVMValueRef Generate(BoundFunctionStatement functionStatement)
    {
        //var paramList = Parameters.Parameters.ToArray();
        var paramList = functionStatement.Parameters.ToList();
        var args = new LLVMTypeRef[paramList.Count];
        for (var i = 0; i < paramList.Count; i++)
        {
            var keyword = KeywordExtensions.FromString(paramList[i].Type.Name);
            var type = keyword.ToLLVMType();
            args[i] = type;
        }

        var functionType = LLVM.FunctionType(LLVMTypeRef.Int32Type(), args, new LLVMBool(0));
        var function = LLVM.AddFunction(Module, functionStatement.Identifier.Name, functionType);
        var block = LLVM.AppendBasicBlock(function, "entry");
        LLVM.PositionBuilderAtEnd(Builder, block);
        for (var i = 0; i < paramList.Count; i++)
        {
            var argument = paramList[i];
            var identifier = argument.Identifier.Name;
            var param = LLVM.GetParam(function, (uint)i);
            LLVM.SetValueName(param, identifier);
        }

        _ = Generate(functionStatement.Statement);
        return function;
    }

    public LLVMValueRef Generate(IBoundStatement statement)
    {
        return statement switch
        {
            BoundReturnStatement ret => Generate(ret),
            BoundBlockStatement block => Generate(block),
            BoundFunctionStatement function => Generate(function),
            _ => throw new NotImplementedException(statement.GetType().FullName)
        };
    }

    public LLVMModuleRef GenerateLlvm()
    {
        var stringType = LLVMTypeRef.PointerType(LLVMTypeRef.Int8Type(), 0);
        var printfArguments = new[] { stringType };
        var printf = LLVM.AddFunction(Module, "printf",
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), printfArguments, new LLVMBool(1)));
        LLVM.SetLinkage(printf, LLVMLinkage.LLVMExternalLinkage);

        var scanfArguments = new[] { stringType };
        var scanf = LLVM.AddFunction(Module, "scanf",
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), scanfArguments, new LLVMBool(0)));
        LLVM.SetLinkage(scanf, LLVMLinkage.LLVMExternalLinkage);

        return Module;
    }

    public void Generate(IEnumerable<IBoundStatement> statements)
    {
        foreach (var boundStatement in statements)
        {
            Generate(boundStatement);
        }
    }
}