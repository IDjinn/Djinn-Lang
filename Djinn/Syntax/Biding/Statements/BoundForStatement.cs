using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundForStatement(
    BoundVariableStatement Variable,
    IBoundExpression Condition,
    IBoundExpression Operation,
    IBoundStatement Block) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.For;
}