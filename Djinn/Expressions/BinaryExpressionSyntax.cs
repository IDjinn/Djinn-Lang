using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record BinaryExpressionSyntax(
    IExpressionSyntax LeftExpression,
    SyntaxToken Operator,
    IExpressionSyntax RightExpression
) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.BinaryExpression;

    public SyntaxToken ReturnType => LeftExpression.ReturnType;

    public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }
}