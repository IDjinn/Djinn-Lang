namespace Djinn.Statements;

public interface IStatementVisitor<T>
{
    public T Visit(DiscardExpressionResultStatement discardExpressionResult);
    public T Visit(FunctionStatement functionStatement);
    public T Visit(ReturnStatement returnStatement);
    public T Visit(BlockStatement blockStatement);
    public T Visit(FunctionDeclarationStatement functionDeclarationStatement);
}