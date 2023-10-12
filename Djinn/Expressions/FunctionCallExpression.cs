using Djinn.Syntax;

namespace Djinn.Expressions;

public readonly record struct FunctionCallExpression(IExpressionSyntax Expression) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.FunctionCallExpression;
    public SyntaxToken ReturnType => Expression.ReturnType;
}