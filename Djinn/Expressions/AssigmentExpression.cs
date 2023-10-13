using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record AssigmentExpression(
    SyntaxToken Identifier,
    SyntaxToken EqualsToken,
    IExpressionSyntax Expression
) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Assignment;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}