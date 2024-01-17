using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record AssigmentExpression(
    SyntaxToken Identifier,
    SyntaxToken EqualsToken,
    IExpressionSyntax Expression
) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"{Identifier.Value} = {Expression.DebugInformationDisplay}";
#endif

    public SyntaxKind Kind => SyntaxKind.Assignment;


    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitAssigmentExpression(this, boundScope);
    }
}