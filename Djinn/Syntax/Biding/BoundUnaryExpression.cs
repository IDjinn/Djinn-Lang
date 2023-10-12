namespace Djinn.Syntax.Biding;

public class BoundUnaryExpression : BoundExpression
{
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public override IType Type => Operand.Type;
    public BoundUnaryOperatorKind OperatorKind { get; init; }
    public BoundExpression Operand { get; init; }
}