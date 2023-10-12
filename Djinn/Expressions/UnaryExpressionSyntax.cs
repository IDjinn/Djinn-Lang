using Djinn.Syntax;

namespace Djinn.Expressions;

public record UnaryExpressionSyntax
    (SyntaxToken ReturnType, SyntaxToken Operator, SyntaxKind Kind) : IExpressionSyntax
{
}