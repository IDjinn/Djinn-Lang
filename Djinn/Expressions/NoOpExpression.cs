using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record NoOpExpression : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Void;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}