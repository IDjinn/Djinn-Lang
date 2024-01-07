using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record ParameterDeclaration(
    SyntaxToken Identifier,
    SyntaxToken Type, 
    IExpressionSyntax? DefaultValue
) : IExpressionSyntax
{
    public static readonly ConstantNumberExpressionSyntax NullDefaultValue =
        new ConstantNumberExpressionSyntax(new(SyntaxKind.Null, "null", new()));
    public static readonly SyntaxToken VoidIdentifier = new SyntaxToken(SyntaxKind.Void, "void", new Position());
    public static readonly ParameterDeclaration VoidParameter = new(VoidIdentifier,VoidIdentifier, NullDefaultValue);
    public static readonly ParameterDeclaration BadParameters = new(VoidIdentifier,VoidIdentifier, NullDefaultValue);
    public SyntaxKind Kind => SyntaxKind.FunctionParametersExpression;


    public T Accept<T>(IExpressionVisitor<T> expr, Scope scope)
    {
        return expr.VisitParameterDeclaration(this,scope);
    }
}