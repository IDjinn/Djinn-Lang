using Djinn.SyntaxNodes;
using Djinn.Utils;

namespace Djinn.Expressions;

public readonly record struct BinaryExpressionSyntax(
    IExpressionSyntax LeftExpression, 
    SyntaxToken Operator, 
    IExpressionSyntax RightExpression
    ) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.BinaryExpression;

    public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }

    public SyntaxToken Type => LeftExpression.Type;
}