using Djinn.Compile;
using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record DiscardExpressionResultStatement(IExpressionSyntax Expression) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.BlockStatement;
    public T Visit<T>(IStatementVisitor<T> visitor, Scope scope)
    {
        return visitor.Visit(this, scope);
    }

}