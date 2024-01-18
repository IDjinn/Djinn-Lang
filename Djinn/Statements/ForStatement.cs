using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record ForStatement(
    VariableDeclarationStatement Variable,
    IExpressionSyntax Condition,
    IExpressionSyntax Operation,
    BlockStatement Block) : IStatement
{
#if DEBUG
    public string DebugInformationDisplay => $"for ";
#endif

    public SyntaxKind Kind => SyntaxKind.For;

    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}