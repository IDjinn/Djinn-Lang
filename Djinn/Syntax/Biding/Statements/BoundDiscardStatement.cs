using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundDiscardStatement(IBoundExpression Expression) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.Discard;
}