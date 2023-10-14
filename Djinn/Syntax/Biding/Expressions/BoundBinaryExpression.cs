using System.Diagnostics;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Binary => [{Left}] {Operator.OperatorKind} [{Right}] return {Type?.GetType().Name}")]
public record BoundBinaryExpression : IBoundExpression
{
    public required IBoundExpression Left { get; init; }
    public required BoundBinaryOperator Operator { get; init; }
    public required IBoundExpression Right { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    public Type Type => Operator.ResultType;
}