using Djinn.SyntaxNodes;

namespace Djinn.Expressions;

public readonly record struct NoOpExpression : IExpressionSyntax
{
    public NoOpExpression()
    {
    }

    public SyntaxToken ReturnType { get; init; } = SyntaxToken.Void;
    public SyntaxKind Kind { get; init; } = SyntaxKind.Void;
}