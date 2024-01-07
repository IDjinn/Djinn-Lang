using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record AssigmentExpression(
    SyntaxToken Identifier,
    SyntaxToken EqualsToken,
    IExpressionSyntax Expression
) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Assignment;

    public T Accept<T>(IExpressionVisitor<T> expr, Scope scope)
    {
        return expr.VisitAssigmentExpression(this,scope);
    }
}