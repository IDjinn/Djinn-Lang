using System.Diagnostics;
using Djinn.Compile;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Unary => ({OperandExpression}) => {Operator.OperatorKind.UnaryOperatorKindToString()} returns {Type.GetType().Name}")]
public record BoundUnaryExpression : IBoundExpression
{
    public required BoundUnaryOperator? Operator { get; init; }
    public required IBoundExpression OperandExpression { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public IType Type => OperandExpression.Type;

    public LLVMValueRef Evaluate(IBoundExpressionVisitor expressionVisitor)
    {
        if (Operator is not null && OperandExpression is BoundLiteralExpression boundLiteralExpression)
        {
            var newValue = new BoundValue()
            {
                Value = Operator.OperatorKind switch
                {
                    BoundUnaryOperatorKind.Identity => new Integer32(+boundLiteralExpression.Value.Value.Value),
                    BoundUnaryOperatorKind.Negation => new Integer32(-boundLiteralExpression.Value.Value.Value),
                    _=> throw new NotImplementedException()
                    
                },
                Type = boundLiteralExpression.Value.Type
            };
            var result = boundLiteralExpression with { Value =newValue };
            return result.Evaluate(expressionVisitor);
        }
        return expressionVisitor.Visit(this);
    }
}