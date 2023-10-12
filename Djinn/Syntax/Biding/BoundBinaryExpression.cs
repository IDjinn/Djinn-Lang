namespace Djinn.Syntax.Biding;

public class BoundBinaryExpression : BoundExpression
{
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public override IType Type => default;
    public required BoundExpression Left { get; init; }
    public required BoundBinaryOperatorKind OperatorKind { get; init; }
    public required BoundExpression Right { get; init; }
}