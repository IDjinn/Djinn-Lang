using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Statements;

namespace Djinn.Statements;

public interface IStatementVisitor<T>
{
    public T Visit(DiscardExpressionResultStatement discardExpressionResult, BoundScope boundScope);
    public T Visit(FunctionStatement functionStatement, BoundScope boundScope);
    public T Visit(ReturnStatement returnStatement, BoundScope boundScope);
    public T Visit(BlockStatement blockStatement, BoundScope boundScope);
    public T Visit(FunctionDeclarationStatement functionDeclarationStatement, BoundScope boundScope);
    public T Visit(IfStatement functionDeclarationStatement, BoundScope boundScope);
    public T Visit(ImportStatement importStatement, BoundScope boundScope);
    public T Visit(SwitchStatement switchStatement, BoundScope boundScope);
}