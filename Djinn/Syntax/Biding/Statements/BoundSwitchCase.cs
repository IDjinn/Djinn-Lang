using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundSwitchCase(
    IBoundExpression ? Expression,
    BoundBlockStatement Block
    ) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.Case;
}