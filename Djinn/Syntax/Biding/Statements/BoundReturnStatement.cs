using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundReturnStatement(IBoundExpression Expression) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.ReturnStatement;
}