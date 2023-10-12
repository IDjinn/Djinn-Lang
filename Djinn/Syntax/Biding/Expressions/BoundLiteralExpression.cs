namespace Djinn.Syntax.Biding;

public record BoundLiteralExpression : IBoundExpression
{
    public required BoundValue Value { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public IType Type => Value.Type;
}