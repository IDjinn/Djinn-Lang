using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record BinaryExpressionSyntax(
    IExpressionSyntax LeftExpression,
    SyntaxToken Operator,
    IExpressionSyntax RightExpression
) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.BinaryExpression;


    public T Accept<T>(IExpressionVisitor<T> expressionVisitor, BoundScope boundScope)
    {
        return expressionVisitor.VisitBinaryExpression(this,boundScope);
    }
}