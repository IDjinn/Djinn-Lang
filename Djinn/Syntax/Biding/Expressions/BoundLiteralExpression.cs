using System.Diagnostics;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Literal => '{Value.Value}' of type '{Type.GetType().Name}'")]
public record BoundLiteralExpression : IBoundExpression
{
    public required BoundValue Value { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public IType Type => Value.Type;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, Scope scope)
    {
        return Type switch
        {
            String str => String.FromValue("a", Value, expressionGenerator.Builder),
            Integer32 integer => Integer32.FromValue("a", Value, expressionGenerator.Builder),
        };
    }
}