namespace Djinn.Syntax.Biding.Expressions;

public record BoundUnaryExpression : IBoundExpression
{
    public required BoundUnaryOperator? Operator { get; init; }
    public required IBoundExpression Operand { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public IType Type => Operand.Type;
}