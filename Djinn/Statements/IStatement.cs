using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public interface IStatement : ISyntaxNode
{
#if DEBUG
    public string DebugInformationDisplay { get; }
#endif
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope);
}