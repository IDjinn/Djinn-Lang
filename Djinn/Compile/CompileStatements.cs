using System.Diagnostics;
using Djinn.Compile.Scopes;
using Djinn.Compile.Variables;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;
using LLVMSharp;

namespace Djinn.Compile;

public static class CompileStatements
{
    public static LLVMValueRef GenerateStatement(CompilationContext ctx, IBoundStatement statement)
    {
        return statement switch
        {
            BoundReturnStatement ret => GenerateReturnStatement(ctx, ret),
            BoundBlockStatement block => GenerateBlockStatement(ctx, block),
            BoundFunctionStatement function => GenerateFunctionStatement(ctx, function),
            BoundDiscardStatement discard => GenerateDiscardStatement(ctx, discard),
            BoundIfStatement ifStatement => GenerateIfStatement(ctx, ifStatement),
            BoundImportStatement importStatement => GenerateImportStatement(ctx, importStatement),
            BoundSwitchStatement switchStatement => GenerateSwitchStatement(ctx, switchStatement),
            BoundVariableStatement variableStatement => GenerateVariableStatement(ctx, variableStatement),
            BoundWhileStatement whileStatement => GenerateWhileStatement(ctx, whileStatement),
            BoundForStatement forStatement => GenerateForStatement(ctx, forStatement),
            _ => throw new NotImplementedException(statement.GetType().FullName)
        };
    }

    private static LLVMValueRef GenerateForStatement(CompilationContext ctx, BoundForStatement forStatement)
    {
        var functionPointer = ctx.Stack.Peek();

        var forBlock = LLVM.AppendBasicBlock(functionPointer, "for");
        var exitBlock = LLVM.AppendBasicBlock(functionPointer, "exit");
        GenerateStatement(ctx, forStatement.Variable);
        LLVM.BuildBr(ctx.Builder, forBlock);
        LLVM.PositionBuilderAtEnd(ctx.Builder, forBlock);

        var value = forStatement.Operation.Generate(ctx);
        var variable = ctx.Scope.FindVariable<LocalVariable>(forStatement.Variable.Name).Value;
        LLVM.BuildStore(ctx.Builder, value, variable.Pointer);
        GenerateStatement(ctx, forStatement.Block);
        LLVMValueRef comparison;
        // TODO: THIS MUST BE DYNAMIC.
        if (forStatement.Condition.Type.GetType().IsInstanceOfType(new Bool()) &&
            forStatement.Condition is BoundBinaryExpression bin) // THIS IS A LOGICAL EXPRESSION
        {
            var left = bin.Left.Generate(ctx);
            var op = bin.Operator.OperatorKind.ToLLVMIntPredicate();
            var right = bin.Right.Generate(ctx);
            comparison = LLVM.BuildICmp(ctx.Builder,
                op,
                left,
                right,
                "cmp");
        }
        else
        {
            var expression = forStatement.Condition.Generate(ctx);
            comparison = LLVM.BuildICmp(ctx.Builder,
                LLVMIntPredicate.LLVMIntEQ,
                expression,
                Integer1.GenerateFromValue((Integer1)1),
                "cmp");
        }

        LLVM.BuildCondBr(ctx.Builder, comparison, forBlock, exitBlock);

        LLVM.PositionBuilderAtEnd(ctx.Builder, exitBlock);
        return default;
    }

    private static LLVMValueRef GenerateWhileStatement(CompilationContext ctx, BoundWhileStatement whileStatement)
    {
        var functionPointer = ctx.Stack.Peek();
        var whileBlock = LLVM.AppendBasicBlock(functionPointer, "while");
        var whileCheck = LLVM.AppendBasicBlock(functionPointer, "while_check");
        var whileExit = LLVM.AppendBasicBlock(functionPointer, "while_exit");

        LLVM.BuildBr(ctx.Builder, whileBlock);
        LLVM.PositionBuilderAtEnd(ctx.Builder, whileBlock);
        GenerateStatement(ctx, whileStatement.Block);

        LLVM.BuildBr(ctx.Builder, whileCheck);
        LLVM.PositionBuilderAtEnd(ctx.Builder, whileCheck);

        LLVMValueRef comparison;
        // TODO: THIS MUST BE DYNAMIC.
        if (whileStatement.Expression.Type.GetType().IsInstanceOfType(new Bool()) &&
            whileStatement.Expression is BoundBinaryExpression bin) // THIS IS A LOGICAL EXPRESSION
        {
            var left = bin.Left.Generate(ctx);
            var op = bin.Operator.OperatorKind.ToLLVMIntPredicate();
            var right = bin.Right.Generate(ctx);
            comparison = LLVM.BuildICmp(ctx.Builder,
                op,
                left,
                right,
                "cmp");
        }
        else
        {
            var expression = whileStatement.Expression.Generate(ctx);
            comparison = LLVM.BuildICmp(ctx.Builder,
                LLVMIntPredicate.LLVMIntEQ,
                expression,
                Integer1.GenerateFromValue((Integer1)1),
                "cmp");
        }

        LLVM.BuildCondBr(ctx.Builder, comparison, whileBlock, whileExit);

        LLVM.PositionBuilderAtEnd(ctx.Builder, whileExit);
        return whileExit;
    }

    private static LLVMValueRef GenerateVariableStatement(CompilationContext ctx,
        BoundVariableStatement variableStatement)
    {
        var value = variableStatement.Value.Generate(ctx);
        var pointer = LLVM.BuildAlloca(ctx.Builder, LLVM.Int32Type(), variableStatement.Name);
        ctx.Scope.TryCreateVariable(new LocalVariable(variableStatement.Name, ctx.Scope, value, pointer,
            new LLVMValueRef()));
        return LLVM.BuildStore(ctx.Builder, value, pointer);
    }

    private static LLVMValueRef GenerateSwitchStatement(CompilationContext ctx, BoundSwitchStatement switchStatement)
    {
        var expression = switchStatement.Expression.Generate(ctx);
        Debug.Assert(expression.Pointer != IntPtr.Zero);

        var functionPointer = ctx.Stack.Peek();
        var defaultSwitch = LLVM.AppendBasicBlock(functionPointer, "default_switch_branch");
        var exitSwitch = LLVM.AppendBasicBlock(functionPointer, "exit");
        var swticher = LLVM.BuildSwitch(ctx.Builder, expression, defaultSwitch, (uint)switchStatement.Cases.Count());
        foreach (var switchStatementCase in switchStatement.Cases)
        {
            if (switchStatementCase.Expression is not null)
            {
                var caseBlock = LLVM.AppendBasicBlock(functionPointer, "case");
                LLVM.PositionBuilderAtEnd(ctx.Builder, caseBlock);
                foreach (var statement in switchStatementCase.Block.Statements)
                {
                    GenerateStatement(ctx, statement);
                }

                LLVM.BuildBr(ctx.Builder, exitSwitch);
                swticher.AddCase(switchStatementCase.Expression.Generate(ctx), caseBlock);
            }
        }

        {
            LLVM.PositionBuilderAtEnd(ctx.Builder, defaultSwitch);
            var defaultCase = switchStatement.Cases.FirstOrDefault(x => x.Expression is null);
            if (defaultCase is not null)
            {
                Debug.Assert(defaultCase is not null);
                foreach (var statement in defaultCase.Block.Statements)
                {
                    GenerateStatement(ctx, statement);
                }
            }

            LLVM.BuildBr(ctx.Builder, exitSwitch);
        }

        LLVM.PositionBuilderAtEnd(ctx.Builder, exitSwitch);
        return exitSwitch;
    }

    private static LLVMValueRef GenerateImportStatement(CompilationContext ctx, BoundImportStatement importStatement)
    {
        LLVM.CreateMemoryBufferWithContentsOfFile(importStatement.TargetLibrary, out var buff, out var error);
        if (error is not null)
            throw new Exception(error);
        LLVM.ParseIRInContext(LLVM.GetGlobalContext(), buff, out var otherModule, out error);
        if (error is not null)
            throw new Exception(error);

        LLVM.SetDataLayout(otherModule, LLVM.GetDataLayout(ctx.Module));
        LLVM.LinkModules2(ctx.Module, otherModule);

        return default;
    }

    private static LLVMValueRef GenerateIfStatement(CompilationContext ctx, BoundIfStatement ifStatement)
    {
        var functionPointer = ctx.Stack.Peek();
        var ifBlock = LLVM.AppendBasicBlock(functionPointer, "if_block");

        LLVMValueRef? elseBlock = default;
        if (ifStatement.Else is not null)
            elseBlock = LLVM.AppendBasicBlock(functionPointer, "else_block");

        var exitBlock = LLVM.AppendBasicBlock(functionPointer, "exit");
        var cnd = ifStatement.Condition.Generate(ctx);
        var condition = LLVM.BuildICmp(ctx.Builder,
            LLVMIntPredicate.LLVMIntEQ,
            cnd,
            Integer1.GenerateFromValue((Integer1)1),
            "cmp");

        LLVM.BuildCondBr(ctx.Builder, condition, ifBlock, elseBlock ?? exitBlock);
        LLVM.PositionBuilderAtEnd(ctx.Builder, ifBlock);
        GenerateStatement(ctx, ifStatement.Block);
        LLVM.BuildBr(ctx.Builder, exitBlock);

        if (ifStatement.Else is not null)
        {
            LLVM.PositionBuilderAtEnd(ctx.Builder, elseBlock!.Value);
            GenerateStatement(ctx, ifStatement.Else);
            LLVM.BuildBr(ctx.Builder, exitBlock);
        }

        LLVM.PositionBuilderAtEnd(ctx.Builder, exitBlock);
        return ifBlock;
    }

    private static LLVMValueRef GenerateDiscardStatement(CompilationContext ctx, BoundDiscardStatement discard)
    {
        var value = discard.Expression.Generate(ctx);
        return default;
    }

    public static LLVMValueRef GenerateReturnStatement(CompilationContext ctx, BoundReturnStatement returnStatement)
    {
        var value = returnStatement.Expression.Generate(ctx);
        return LLVM.BuildRet(ctx.Builder, value);
    }

    public static LLVMValueRef GenerateBlockStatement(CompilationContext ctx, BoundBlockStatement blockStatement)
    {
        var block = new LLVMValueRef();
        foreach (var statement in blockStatement.Statements)
            block = GenerateStatement(ctx, statement);

        return block;
    }

    public static LLVMValueRef GenerateFunctionStatement(CompilationContext ctx,
        BoundFunctionStatement functionStatement)
    {
        var functionScope = new CompilationFunctionScope(functionStatement.Identifier.Name, ctx.Scope);
        var newCtx = ctx with { Scope = functionScope };

        var parameterTypes = GenerateFunctionParameterTypes(newCtx, functionStatement);
        var functionSignature =
            LLVM.FunctionType(LLVMTypeRef.Int32Type(), parameterTypes,
                new LLVMBool(0)); // TODO: RETURN TYPE BY DECLARATION
        var function = LLVM.AddFunction(ctx.Module, functionStatement.Identifier.Name, functionSignature);
        var block = LLVM.AppendBasicBlock(function, "entry");
        ctx.Stack.Push(function);
        LLVM.PositionBuilderAtEnd(ctx.Builder, block);

        var parameterPointers = GenerateFunctionParameterNames(newCtx, functionStatement, function);
        var functionBody = GenerateBlockStatement(newCtx, (BoundBlockStatement)functionStatement.Statement);
        LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);
        ctx.Scope.TryCreateFunction(functionStatement.Identifier.Name, function);
        return function;
    }

    public static LLVMTypeRef[] GenerateFunctionParameterTypes(CompilationContext ctx,
        BoundFunctionStatement function)
    {
        var parameters = function.Parameters.ToList();
        var parametersResult = new LLVMTypeRef[parameters.Count];
        for (var i = 0; i < parameters.Count; i++)
        {
            var keyword = KeywordExtensions.FromString(parameters[i].Type.Name); // TODO: PARSE BY SCOPE
            var type = keyword.ToLLVMType();
            parametersResult[i] = type;
        }

        return parametersResult;
    }

    public static LLVMValueRef[] GenerateFunctionParameterNames(
        CompilationContext ctx,
        BoundFunctionStatement function,
        LLVMValueRef functionPointer
    )
    {
        if (ctx.Scope is not CompilationFunctionScope fnScope)
            throw new ArgumentException("Invalid call, must be used just for function declarations.");

        var parameters = function.Parameters.ToList();
        var parameterPointers = new LLVMValueRef[parameters.Count];
        for (var i = 0; i < parameters.Count; i++)
        {
            var argument = parameters[i];
            var identifier = argument.Identifier.Name;
            var parameterPointer = LLVM.GetParam(functionPointer, (uint)i);
            parameterPointers[i] = parameterPointer;
            LLVM.SetValueName(parameterPointer, identifier);
            fnScope.AddParameter(new ParameterVariable
            {
                Identifier = identifier,
                FunctionPointer = functionPointer,
                Pointer = parameterPointer,
                Scope = ctx.Scope,
            });

            ctx.Stack.Push(parameterPointer);
        }

        return parameterPointers;
    }
}