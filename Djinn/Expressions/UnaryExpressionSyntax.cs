using Djinn.Syntax;

namespace Djinn.Expressions;

public readonly record struct UnaryExpressionSyntax
    (SyntaxToken ReturnType, SyntaxToken Operator, SyntaxKind Kind) : IExpressionSyntax
{
}