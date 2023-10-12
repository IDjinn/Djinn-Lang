namespace Djinn.Syntax.Biding;

public record BoundUnaryExpression : IBoundExpression
{
    public BoundUnaryOperatorKind OperatorKind { get; init; }
    public IBoundExpression Operand { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public IType Type => Operand.Type;
}