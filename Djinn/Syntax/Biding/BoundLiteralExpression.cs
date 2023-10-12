namespace Djinn.Syntax.Biding;

public class BoundLiteralExpression : BoundExpression
{
    public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public override IType Type => Value.Type;
    public required BoundValue Value { get; init; }
}