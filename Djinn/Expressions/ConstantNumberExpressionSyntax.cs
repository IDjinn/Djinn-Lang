using Djinn.SyntaxNodes;
using Djinn.Utils;

namespace Djinn.Expressions;

public readonly record struct ConstantNumberExpressionSyntax(SyntaxToken NumberToken) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ConstantNumberExpression;
    
    public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }

    public SyntaxToken Type => NumberToken;
}