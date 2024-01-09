using System.Diagnostics;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Literal => '{Number.Value}' of type '{Type.GetType().Name}'")]
public record BoundConstantBooleanLiteralExpression(
    Bool Value
    ) : IBoundExpression 
{
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public IType Type => Value;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        return Bool.GenerateFromValue(Value);
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        return Bool.GenerateFromValue(Value);
    }
}