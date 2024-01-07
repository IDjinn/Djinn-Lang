using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record UnaryExpressionSyntax(IExpressionSyntax Operand, SyntaxToken Operator) : IExpressionSyntax
{
    public T Accept<T>(IExpressionVisitor<T> expr, Scope scope)
    {
        return expr.VisitUnaryExpression(this,scope);
    }

    public SyntaxKind Kind => SyntaxKind.UnaryExpression;
}