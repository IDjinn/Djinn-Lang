using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Utils;

namespace Djinn.Syntax.Biding;

public class Binder : IStatementVisitor<object>, IExpressionVisitor<object>
{
    public object Visit(BinaryExpressionSyntax expressionSyntax)
    {
        return BindBinaryExpression(expressionSyntax);
    }

    public object Visit(ConstantNumberExpressionSyntax expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public object Visit(ConstantStringExpressionSyntax expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public object Visit(FunctionStatement functionStatement)
    {
        throw new NotImplementedException();
    }

    public object Visit(ReturnStatement returnStatement)
    {
        throw new NotImplementedException();
    }

    public BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax)
    {
        return new BoundBinaryExpression()
        {
            Left = BindExpression(binaryExpressionSyntax.LeftExpression),
            OperatorKind = binaryExpressionSyntax.Operator.Kind.BindBinaryOperatorKind(),
            Right = BindExpression(binaryExpressionSyntax.LeftExpression)
        };
    }

    public BoundExpression BindExpression(IExpressionSyntax expressionSyntax)
    {
        return expressionSyntax switch
        {
            BinaryExpressionSyntax binary => BindBinaryExpression(binary),
            UnaryExpressionSyntax unary => BindUnaryExpression(unary),
            ConstantNumberExpressionSyntax constantNumber => BindLiteralNumber(constantNumber),
            ConstantStringExpressionSyntax constantString => BindLiteralString(constantString),
            _ => throw new InvalidOperationException()
        };
    }

    private BoundExpression BindLiteralString(ConstantStringExpressionSyntax constantString)
    {
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Value = constantString.StringToken.Value,
                Type = new String(constantString.StringToken.Value.ToString()!)
            }
        };
    }

    private BoundExpression BindLiteralNumber(ConstantNumberExpressionSyntax constantNumber)
    {
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Value = constantNumber.NumberToken.Value,
                Type = new Integer32((int)constantNumber.NumberToken.Value)
            }
        };
    }

    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        return new BoundUnaryExpression()
        {
            Operand = BindExpression(unary),
            OperatorKind = unary.Operator.Kind.BindUnaryOperatorKind(),
        };
    }
}