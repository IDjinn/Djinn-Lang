namespace Djinn.Statements;

public interface IStatementGenerator<T>
{
    public T Generate(DiscardExpressionResultStatement discardExpressionResult);
    public T Generate(ReturnStatement returnStatement);
    public T Generate(BlockStatement blockStatement);
    public T Generate(FunctionDeclarationStatement functionDeclarationStatement);
}