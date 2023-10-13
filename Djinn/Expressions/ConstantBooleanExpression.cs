using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ConstantBooleanExpression(SyntaxToken Bool) : IExpressionSyntax
{
    public SyntaxKind Kind => Bool.Kind;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}