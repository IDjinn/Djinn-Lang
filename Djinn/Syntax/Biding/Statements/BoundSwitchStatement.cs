using Djinn.Statements;
using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundSwitchStatement(
    IBoundExpression Expression,
    IEnumerable<BoundSwitchCase> Cases
    ) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.Switch;
}