using Djinn.Compile;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public interface IStatement : ISyntaxNode
{
    public T Visit<T>(IStatementVisitor<T> visitor, Scope scope);

}