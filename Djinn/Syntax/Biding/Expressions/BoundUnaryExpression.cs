using System.Diagnostics;
using Djinn.Compile;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Unary => ({Operand}) => {Operator.OperatorKind} returns {Type.GetType().Name}")]
public record BoundUnaryExpression : IBoundExpression
{
    public required BoundUnaryOperator? Operator { get; init; }
    public required IBoundExpression Operand { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public IType Type => Operand.Type;

    public LLVMValueRef Evaluate(IBoundExpressionVisitor expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }
}