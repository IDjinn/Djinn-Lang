using Djinn.Syntax;

namespace Djinn.Expressions;

public record ConstantBooleanExpression(SyntaxToken Bool) : IExpressionSyntax
{
    public SyntaxKind Kind => Bool.Kind;
}