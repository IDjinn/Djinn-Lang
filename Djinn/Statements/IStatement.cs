using Djinn.Compile;
using Djinn.Syntax;

namespace Djinn.Statements;

public interface IStatement : ISyntaxNode
{
    public T Visit<T>(IStatementVisitor<T> visitor);

    public T Generate<T>(IStatementVisitor<T> visitor, CodeGen codeGen);
}