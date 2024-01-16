using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundWhileStatement(IBoundExpression Expression, BoundBlockStatement Block) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.While;
}