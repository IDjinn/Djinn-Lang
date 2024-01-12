using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundIfStatement(IBoundExpression Condition, IBoundStatement Block) :IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.If;
}