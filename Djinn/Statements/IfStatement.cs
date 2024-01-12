using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record IfStatement(
    IExpressionSyntax Conditional, // TODO MAKE THIS CONDITIONAL AT COMPILE TIME
    IStatement Block
    ) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.IfStatement;
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}