using Djinn.SyntaxNodes;

namespace Djinn.Expressions;

public readonly record struct FunctionCallExpression(IExpressionSyntax Expression): IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.FunctionCallExpression;
    public SyntaxToken Type => Expression.Type;
}