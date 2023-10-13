using Djinn.Syntax;

namespace Djinn.Expressions;

public record VariableExpression(SyntaxToken Identifier) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.Variable;
}