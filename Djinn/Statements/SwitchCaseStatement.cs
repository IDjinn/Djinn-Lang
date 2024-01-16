using Djinn.Expressions;
using Djinn.Statements;

namespace Djinn.Syntax.Biding.Statements;

public record SwitchCaseStatement(
    IExpressionSyntax? Expression,
    BlockStatement Block
) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.Case;
}