using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public interface IExpressionSyntax : ISyntaxNode
{
#if DEBUG
    public string DebugInformationDisplay { get; }
#endif
    T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope);
}