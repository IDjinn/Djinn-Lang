using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public interface IStatementVisitor<T>
{
    public T Visit(DiscardExpressionResultStatement discardExpressionResult, Scope scope);
    public T Visit(FunctionStatement functionStatement, Scope scope);
    public T Visit(ReturnStatement returnStatement, Scope scope);
    public T Visit(BlockStatement blockStatement, Scope scope);
    public T Visit(FunctionDeclarationStatement functionDeclarationStatement, Scope scope);
}