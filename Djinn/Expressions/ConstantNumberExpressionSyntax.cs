using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record ConstantNumberExpressionSyntax(SyntaxToken NumberToken) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"const {NumberToken.Value}";
#endif

    public SyntaxKind Kind => SyntaxKind.ConstantNumberExpression;


    public T Accept<T>(IExpressionVisitor<T> expressionVisitor, BoundScope boundScope)
    {
        return expressionVisitor.VisitConstantNumberExpression(this, boundScope);
    }
}