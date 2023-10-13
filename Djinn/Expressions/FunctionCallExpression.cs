using Djinn.Syntax;

namespace Djinn.Expressions;

public record FunctionCallExpression(IExpressionSyntax Expression) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.FunctionCallExpression;
}