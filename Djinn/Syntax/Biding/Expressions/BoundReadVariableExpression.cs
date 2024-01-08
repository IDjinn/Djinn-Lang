using Djinn.Compile;
using Djinn.Compile.Scopes;
using Djinn.Compile.Variables;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Scopes.Variables;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

public record BoundReadVariableExpression(BoundVariable BoundVariable) : IBoundExpression
{
    public BoundNodeKind Kind => BoundNodeKind.ReadVariable;
    public IType Type => BoundVariable.Type;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        var boundVariable = boundScope.FindVariable(BoundVariable.Identifier);
        if(!boundVariable.HasValue)  throw new NotImplementedException();

        var variable = boundVariable.Value;
        // var a = LLVM.GetNamedFunction(expressionGenerator.Module,"main");
        // var b = LLVM.BuildLoad(expressionGenerator.Builder,variable.Pointer.Value, "olocomeu");
        // var test =LLVM.BuildAdd(expressionGenerator.Builder, LLVM.GetParam(function, (uint)0), LLVM.GetParam(function, (uint)1),"test")
        return default;
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        if (ctx.Scope is CompilationFunctionScope fnScope)
        {
            var parameter = fnScope.FindVariable<ParameterVariable>(BoundVariable.Identifier);


            return parameter.Pointer;
        }
        throw new NotImplementedException();
    }
}