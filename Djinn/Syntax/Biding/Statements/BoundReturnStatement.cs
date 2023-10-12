namespace Djinn.Syntax.Biding.Statements;

public record BoundReturnStatement(IBoundExpression Expression) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.ReturnStatement;
}