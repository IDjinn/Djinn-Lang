using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record FunctionCallExpression(SyntaxToken Identifier, IEnumerable<IExpressionSyntax> Arguments) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.FunctionCallExpression;

    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitFunctionCallExpression(this,boundScope);
    }
}