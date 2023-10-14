using System.Diagnostics;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Literal => '{Value.Value}' of type '{Type.GetType().Name}'")]
public record BoundLiteralExpression : IBoundExpression
{
    public required BoundValue Value { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public Type Type => Value.Type;
}