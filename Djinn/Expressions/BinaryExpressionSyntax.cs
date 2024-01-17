using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record BinaryExpressionSyntax(
    IExpressionSyntax LeftExpression,
    SyntaxToken Operator,
    IExpressionSyntax RightExpression
) : IExpressionSyntax
{
    public SyntaxKind Kind => SyntaxKind.BinaryExpression;


#if DEBUG
    public string DebugInformationDisplay =>
        $"{LeftExpression.DebugInformationDisplay} {Operator.Value} {RightExpression.DebugInformationDisplay}";
#endif
    public T Accept<T>(IExpressionVisitor<T> expressionVisitor, BoundScope boundScope)
    {
        return expressionVisitor.VisitBinaryExpression(this, boundScope);
    }
}