using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record ArgumentsExpression(IEnumerable<IExpressionSyntax> Arguments) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay =>
        $"({string.Join(", ", Arguments.Select(arg => arg.DebugInformationDisplay))})";
#endif

    public SyntaxKind Kind => SyntaxKind.FunctionArgumentsExpression;


    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitArgumentsExpression(this, boundScope);
    }
}