using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record VariableDeclarationStatement(
    SyntaxToken Type,
    SyntaxToken Identifier,
    IExpressionSyntax Expression)
    : IStatement
{
#if DEBUG
    public string DebugInformationDisplay => $"{Type.Value} {Identifier.Value} = {Expression.DebugInformationDisplay}";
#endif

    public SyntaxKind Kind => SyntaxKind.LocalVariableDeclaration;

    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }

    public static IExpressionSyntax ConstantDefaultValueOfType(SyntaxToken type)
    {
        var value = (string)type.Value;
        return value switch
        {
            "int32" => new ConstantNumberExpressionSyntax(type with { Value = Integer32.DefaultValue }),
            "int1" => new ConstantNumberExpressionSyntax(type with { Value = Integer1.DefaultValue }),
            "true" => new ConstantBooleanExpression(type with { Value = "true" }),
            "false" => new ConstantBooleanExpression(type with { Value = "false" }),
            _ => throw new NotImplementedException()
        };
    }
}