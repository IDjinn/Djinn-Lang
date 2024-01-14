using Djinn.Compile.Scopes;
using Djinn.Compile.Variables;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
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
            BoundDiscardStatement discard => GenerateDiscardStatement(ctx,discard),
            BoundIfStatement ifStatement => GenerateIfStatement(ctx, ifStatement),
            BoundImportStatement importStatement => GenerateImportStatement(ctx, importStatement),
            _ => throw new NotImplementedException(statement.GetType().FullName)
        };
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
        var elseBlock = LLVM.AppendBasicBlock(functionPointer, "else_block");
        var cnd = ifStatement.Condition.Generate(ctx);
        var condition = LLVM.BuildICmp(ctx.Builder,
            LLVMIntPredicate.LLVMIntEQ,
            cnd,
            Integer1.GenerateFromValue((Integer1)1),
            "cmp");
        
        LLVM.BuildCondBr(ctx.Builder, condition, ifBlock,elseBlock);
        LLVM.PositionBuilderAtEnd(ctx.Builder, ifBlock);
        GenerateStatement(ctx, ifStatement.Block);

        if (ifStatement.ElseBlock is IBoundStatement elseBranch)
        {
            LLVM.PositionBuilderAtEnd(ctx.Builder, elseBlock);
            GenerateStatement(ctx, elseBranch);
        }
        
        LLVM.PositionBuilderAtEnd(ctx.Builder, elseBlock);
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

    public static LLVMValueRef GenerateFunctionStatement(CompilationContext ctx, BoundFunctionStatement functionStatement)
    {
        var functionScope = new CompilationFunctionScope(functionStatement.Identifier.Name,ctx.Scope);
        var newCtx = ctx with { Scope = functionScope };

        var parameterTypes = GenerateFunctionParameterTypes(newCtx, functionStatement);
        var functionSignature = LLVM.FunctionType(LLVMTypeRef.Int32Type(), parameterTypes, new LLVMBool(0)); // TODO: RETURN TYPE BY DECLARATION
        var function = LLVM.AddFunction(ctx.Module, functionStatement.Identifier.Name, functionSignature);
        var block = LLVM.AppendBasicBlock(function, "entry");
        ctx.Stack.Push(function);
        LLVM.PositionBuilderAtEnd(ctx.Builder, block);
        
        var parameterPointers = GenerateFunctionParameterNames(newCtx, functionStatement, function);
        var functionBody = GenerateBlockStatement(newCtx, (BoundBlockStatement)functionStatement.Statement);
        LLVM.VerifyFunction(function, LLVMVerifierFailureAction.LLVMPrintMessageAction);
        ctx.Scope.TryCreateFunction(functionStatement.Identifier.Name,function);
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