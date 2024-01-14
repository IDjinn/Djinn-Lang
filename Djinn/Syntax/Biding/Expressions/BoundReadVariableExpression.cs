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
        return default;
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        if (ctx.Scope is CompilationFunctionScope fnScope)
        {
            var parameter = fnScope.FindVariable<ParameterVariable>(BoundVariable.Identifier);
            if (parameter.HasValue)
                return parameter.Value.Pointer;
        }

        var variable = ctx.Scope.FindVariable<LocalVariable>(BoundVariable.Identifier);
        if (variable.HasValue)
            return LLVM.BuildLoad(ctx.Builder, variable.Value.Pointer, "read");
        
        throw new NotImplementedException();
    }
}