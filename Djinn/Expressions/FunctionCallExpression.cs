using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record FunctionCallExpression(SyntaxToken Identifier, IEnumerable<IExpressionSyntax> Arguments)
    : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay =>
        $"call {Identifier.Value} ({string.Join(", ", Arguments.Select(x => x.DebugInformationDisplay))})";
#endif
    public SyntaxKind Kind => SyntaxKind.FunctionCallExpression;

    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitFunctionCallExpression(this, boundScope);
    }
}