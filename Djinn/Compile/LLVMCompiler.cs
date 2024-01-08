using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Scopes.Variables;
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

    public LLVMValueRef Generate(IBoundExpression boundExpression, BoundScope boundScope)
    {
        return boundExpression switch
        {
            BoundLiteralExpression literal => GenerateLiteralExpression(literal, boundScope),
            BoundUnaryExpression unary => GenerateUnaryExpression(unary, boundScope),
            BoundBinaryExpression binary => GenerateBinaryExpression(binary, boundScope),
            BoundReadVariableExpression readVariableExpression => GenerateReadVariableExpression(readVariableExpression,boundScope),
            _ => throw new NotImplementedException()
        };
    }

    private LLVMValueRef GenerateReadVariableExpression(BoundReadVariableExpression readVariableExpression, BoundScope boundScope)
    {
        return readVariableExpression.Evaluate(this, boundScope);
    }

    public LLVMValueRef GenerateBinaryExpression(BoundBinaryExpression boundBinaryExpression, BoundScope boundScope)
    {
        return boundBinaryExpression.Evaluate(this, boundScope);
    }

    public LLVMValueRef GenerateUnaryExpression(BoundUnaryExpression boundUnaryExpression, BoundScope boundScope)
    {
        var value = boundUnaryExpression.Evaluate(this, boundScope);
        return value;
    }

    public LLVMValueRef GenerateLiteralExpression(BoundLiteralExpression boundLiteralExpression, BoundScope boundScope)
    {
        var value = boundLiteralExpression.Evaluate(this, boundScope);
        return value;
    }


    public LLVMValueRef GenerateReturnStatement(BoundReturnStatement returnStatement, BoundScope boundScope)
    {
        var value = returnStatement.Expression.Evaluate(this, boundScope);
        return LLVM.BuildRet(Builder, value);
    }

    public LLVMValueRef GenerateBlockStatement(BoundBlockStatement blockStatement, BoundScope boundScope)
    {
        BoundScope blockBoundScope = boundScope is BoundFunctionScope ? boundScope : new BoundScope("block-scope", boundScope);
        LLVMValueRef block = new LLVMValueRef();
        foreach (var boundStatement in blockStatement.Statements)
        {
            block = GenerateStatement(boundStatement, blockBoundScope);
        }

        return block;
    }

    public LLVMValueRef GenerateFunctionStatement(BoundFunctionStatement functionStatement, BoundFunctionScope scope)
    {
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
            // scope.AddParameter(new BoundVariable(paramList[i].Identifier.Name, scope.FindType(paramList[i].Type.Name).Value.Type, scope, param));
        }

        _ = GenerateStatement(functionStatement.Statement, scope);
        return function;
    }

    public LLVMValueRef GenerateStatement(IBoundStatement statement, BoundScope boundScope)
    {
        return statement switch
        {
            BoundReturnStatement ret => GenerateReturnStatement(ret, boundScope),
            BoundBlockStatement block => GenerateBlockStatement(block, boundScope),
            BoundFunctionStatement function => GenerateFunctionStatement(function, new BoundFunctionScope(function.Identifier.Name, boundScope)),
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
        var scope = new BoundScope("global");
        foreach (var boundStatement in statements)
        {
            GenerateStatement(boundStatement, scope);
        }
    }
}