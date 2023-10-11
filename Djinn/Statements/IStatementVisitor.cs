namespace Djinn.Statements;

public interface IStatementVisitor<T>
{
    public T Visit(FunctionStatement functionStatement);
    public T Visit(ReturnStatement returnStatement);
}