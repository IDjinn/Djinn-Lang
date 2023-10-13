using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ParameterExpression(
    SyntaxToken Identifier,
    SyntaxToken? DefaultValue
) : IExpressionSyntax
{
    public static readonly SyntaxToken VoidIdentifier = new SyntaxToken(SyntaxKind.Void, "void", new Position());
    public static readonly ParameterExpression VoidParameters = new(VoidIdentifier, null);
    public static readonly ParameterExpression BadParameters = new(VoidIdentifier, null);
    public SyntaxKind Kind => SyntaxKind.FunctionParametersExpression;
}