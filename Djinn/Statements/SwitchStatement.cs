using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Syntax.Biding.Statements;

public record SwitchStatement( 
    IExpressionSyntax Expression,
    IEnumerable<SwitchCaseStatement> Cases
        ) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.Switch;
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }

}