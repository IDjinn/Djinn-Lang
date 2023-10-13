using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record NoOpExpression : IExpressionSyntax
{
    public NoOpExpression()
    {
    }

    public SyntaxKind Kind { get; init; } = SyntaxKind.Void;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}