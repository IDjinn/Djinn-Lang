using Djinn.Syntax;

namespace Djinn.Expressions;

public record NoOpExpression : IExpressionSyntax
{
    public NoOpExpression()
    {
    }

    public SyntaxKind Kind { get; init; } = SyntaxKind.Void;
}