using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record IdentifierExpression(SyntaxToken Identifier) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Identifier;

    public T Accept<T>(IExpressionVisitor<T> expr, Scope scope)
    {
        return expr.VisitIdentifierExpression(this,scope);
    }
}