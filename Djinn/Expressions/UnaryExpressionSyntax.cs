using Djinn.Syntax;

namespace Djinn.Expressions;

public readonly record struct UnaryExpressionSyntax(SyntaxToken ReturnType, SyntaxKind Kind) : IExpressionSyntax
{
}