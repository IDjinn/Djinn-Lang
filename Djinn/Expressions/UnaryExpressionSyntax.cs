using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record UnaryExpressionSyntax(IExpressionSyntax Operand, SyntaxToken Operator) : IExpressionSyntax
{
    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }

    public SyntaxKind Kind => SyntaxKind.UnaryExpression;
}