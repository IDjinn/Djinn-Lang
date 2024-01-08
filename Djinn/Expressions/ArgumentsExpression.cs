using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ArgumentsExpression(IEnumerable<IExpressionSyntax> Arguments) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.FunctionArgumentsExpression;

    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitArgumentsExpression(this,boundScope);
    }
}