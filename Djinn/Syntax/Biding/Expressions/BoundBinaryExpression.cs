namespace Djinn.Syntax.Biding.Expressions;

public record BoundBinaryExpression : IBoundExpression
{
    public required IBoundExpression Left { get; init; }
    public required BoundBinaryOperator? Operator { get; init; }
    public required IBoundExpression Right { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    public IType Type => default;
}