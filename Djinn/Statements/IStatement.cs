using Djinn.SyntaxNodes;

namespace Djinn.Statements;

public interface IStatement : ISyntaxNode
{
    public T Visit<T>(IStatementVisitor<T> visitor);

    public T Generate<T>(IStatementVisitor<T> visitor);
}