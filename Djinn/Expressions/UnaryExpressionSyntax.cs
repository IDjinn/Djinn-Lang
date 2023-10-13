using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record UnaryExpressionSyntax(SyntaxToken Operator, SyntaxKind Kind) : IExpressionSyntax
{
    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}