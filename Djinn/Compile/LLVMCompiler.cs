using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;
using LLVMSharp;
using Microsoft.CodeAnalysis;

namespace Djinn.Compile;

public class LLVMCompiler : IBoundExpressionGenerator, IBoundStatementGenerator
{
    private readonly IEnumerable<IBoundStatement> _syntaxTree;

    public Stack<KeyValuePair<LLVMValueRef, LLVMValueRef>>
        Stack = new Stack<KeyValuePair<LLVMValueRef, LLVMValueRef>>();

    public LLVMCompiler(IEnumerable<IBoundStatement> syntaxTree)
    {
        _syntaxTree = syntaxTree;
        Module = LLVM.ModuleCreateWithName("main");
        Builder = LLVM.CreateBuilder();
        Context = LLVM.ContextCreate();
    }

    public void Compile()
    {
        GenerateLlvm();
        GenerateStatements(_syntaxTree);
    }

    public LLVMModuleRef Module { get; init; }
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }

    public LLVMValueRef Generate(IBoundExpression boundExpression, Scope scope)
    {
        return boundExpression switch
        {
            BoundLiteralExpression literal => GenerateLiteralExpression(literal, scope),
            BoundUnaryExpression unary => GenerateUnaryExpression(unary, scope),
            BoundBinaryExpression binary => GenerateBinaryExpression(binary, scope),
            _ => throw new NotImplementedException()
        };
    }

    public LLVMValueRef GenerateBinaryExpression(BoundBinaryExpression boundBinaryExpression, Scope scope)
    {
        return boundBinaryExpression.Evaluate(this, scope);
    }

    public LLVMValueRef GenerateUnaryExpression(BoundUnaryExpression boundUnaryExpression, Scope scope)
    {
        var value = boundUnaryExpression.Evaluate(this, scope);
        return value;
    }

    public LLVMValueRef GenerateLiteralExpression(BoundLiteralExpression boundLiteralExpression, Scope scope)
    {
        var value = boundLiteralExpression.Evaluate(this, scope);
        return value;
    }


    public LLVMValueRef GenerateReturnStatement(BoundReturnStatement returnStatement, Scope scope)
    {
        var value = returnStatement.Expression.Evaluate(this, scope);
        return LLVM.BuildRet(Builder, value);
    }

    public LLVMValueRef GenerateBlockStatement(BoundBlockStatement blockStatement, Scope scope)
    {
        LLVMValueRef block = new LLVMValueRef();
        foreach (var boundStatement in blockStatement.Statements)
        {
            block = GenerateStatement(boundStatement, scope);
        }

        return block;
    }

    public LLVMValueRef GenerateFunctionStatement(BoundFunctionStatement functionStatement, FunctionScope scope)
    {
        //var paramList = Parameters.Parameters.ToArray();
        var paramList = functionStatement.Parameters.ToList();
        var args = new LLVMTypeRef[paramList.Count];
        for (var i = 0; i < paramList.Count; i++)
        {
            var keyword = KeywordExtensions.FromString(paramList[i].Type.Name);
            var type = keyword.ToLLVMType();
            args[i] = type;
            scope.TryAddParameter(paramList[i]);
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

        _ = GenerateStatement(functionStatement.Statement, scope);
        return function;
    }

    public LLVMValueRef GenerateStatement(IBoundStatement statement, Scope scope)
    {
        return statement switch
        {
            BoundReturnStatement ret => GenerateReturnStatement(ret, scope),
            BoundBlockStatement block => GenerateBlockStatement(block, new Scope("block", scope)),
            BoundFunctionStatement function => GenerateFunctionStatement(function, new FunctionScope(function.Identifier.Name, scope)),
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

    public void GenerateStatements(IEnumerable<IBoundStatement> statements)
    {
        var scope = new Scope("global");
        foreach (var boundStatement in statements)
        {
            GenerateStatement(boundStatement, scope);
        }
    }
}