using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ConstantStringExpressionSyntax(SyntaxToken StringToken) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ConstantStringExpression;


    public T Accept<T>(IExpressionVisitor<T> expressionVisitor, BoundScope boundScope)
    {
        return expressionVisitor.VisitConstantStringExpression(this,boundScope);
    }
}