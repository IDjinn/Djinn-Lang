using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record FunctionCallExpression(SyntaxToken Identifier, IExpressionSyntax Expression) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.FunctionCallExpression;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}