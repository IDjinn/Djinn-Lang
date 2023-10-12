using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public readonly record struct ConstantNumberExpressionSyntax(SyntaxToken NumberToken) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.ConstantNumberExpression;

    public SyntaxToken ReturnType => NumberToken;

    public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
    {
        return expressionVisitor.Visit(this);
    }
}