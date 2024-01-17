using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record ConstantBooleanExpression(SyntaxToken Bool) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"const {Bool.Value}";
#endif
    public SyntaxKind Kind => Bool.Kind;

    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitConstantBooleanExpression(this, boundScope);
    }
}