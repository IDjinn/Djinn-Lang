using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record ConstantStringExpressionSyntax(SyntaxToken StringToken) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"str {StringToken.Value}";
#endif
    public SyntaxKind Kind => SyntaxKind.ConstantStringExpression;


    public T Accept<T>(IExpressionVisitor<T> expressionVisitor, BoundScope boundScope)
    {
        return expressionVisitor.VisitConstantStringExpression(this, boundScope);
    }
}