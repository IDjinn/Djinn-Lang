using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record NameExpression(SyntaxToken Identifier) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Identifier;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}