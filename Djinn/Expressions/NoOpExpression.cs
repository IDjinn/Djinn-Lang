using Djinn.Syntax;

namespace Djinn.Expressions;

public record NoOpExpression : IExpressionSyntax
{
    public NoOpExpression()
    {
    }

    public SyntaxToken ReturnType { get; init; } = SyntaxToken.Void;
    public SyntaxKind Kind { get; init; } = SyntaxKind.Void;
}