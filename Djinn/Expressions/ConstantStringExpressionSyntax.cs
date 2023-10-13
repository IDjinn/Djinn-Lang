using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ConstantStringExpressionSyntax(SyntaxToken StringToken) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ConstantStringExpression;


    public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }
}