using Djinn.SyntaxNodes;
using Djinn.Utils;

namespace Djinn.Expressions;

public readonly record struct ConstantStringExpressionSyntax(SyntaxToken StringToken) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ConstantStringExpression;
    
    
    public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }

    public SyntaxToken Type => StringToken;
}