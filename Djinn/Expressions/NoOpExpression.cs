using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record NoOpExpression : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"nop";
#endif
    public SyntaxKind Kind => SyntaxKind.Void;

    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitNoOpExpression(this, boundScope);
    }
}