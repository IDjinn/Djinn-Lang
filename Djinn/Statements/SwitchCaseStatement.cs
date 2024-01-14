using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record SwitchCaseStatement(
    IExpressionSyntax? Expression,
    BlockStatement Block
    ) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.Case;
}