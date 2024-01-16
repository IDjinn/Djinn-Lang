using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record DiscardExpressionResultStatement(IExpressionSyntax Expression) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.BlockStatement;

    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}