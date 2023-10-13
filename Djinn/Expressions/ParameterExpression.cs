using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ParameterExpression(
    SyntaxToken Identifier,
    SyntaxToken? DefaultValue
) : IExpressionSyntax
{
    public static readonly SyntaxToken VoidIdentifier = new SyntaxToken(SyntaxKind.Void, "void", new Position());
    public static readonly ParameterExpression VoidParameters = new(VoidIdentifier, VoidIdentifier);
    public static readonly ParameterExpression BadParameters = new(VoidIdentifier, VoidIdentifier);
    public SyntaxKind Kind => SyntaxKind.FunctionParametersExpression;

    public T Accept<T>(IExpressionVisitor<T> expr)
    {
        return expr.Visit(this);
    }
}