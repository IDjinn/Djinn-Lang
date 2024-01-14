using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ReadVariableExpression(
    SyntaxToken Identifier,
    SyntaxToken Type // TODO:STATIC OR DYNAMIC ?
    ) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ReadVariable;
    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitReadVariableExpression(this, boundScope);
    }
}