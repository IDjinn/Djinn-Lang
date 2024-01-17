using System.Diagnostics;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

[DebuggerDisplay("{DebugInformationDisplay}")]
public record IdentifierExpression(SyntaxToken Identifier) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"{Identifier.Value}";
#endif

    public SyntaxKind Kind => SyntaxKind.Identifier;

    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitIdentifierExpression(this, boundScope);
    }
}