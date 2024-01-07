using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record NoOpExpression : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Void;

    public T Accept<T>(IExpressionVisitor<T> expr, Scope scope)
    {
        return expr.VisitNoOpExpression(this,scope);
    }
}