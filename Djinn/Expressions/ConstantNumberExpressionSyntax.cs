using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ConstantNumberExpressionSyntax(SyntaxToken NumberToken) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ConstantNumberExpression;


    public T Accept<T>(IExpressionVisitor<T> expressionVisitor, BoundScope boundScope)
    {
        return expressionVisitor.VisitConstantNumberExpression(this,boundScope);
    }
}