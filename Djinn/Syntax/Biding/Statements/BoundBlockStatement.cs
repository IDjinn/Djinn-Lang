namespace Djinn.Syntax.Biding.Statements;

public record BoundBlockStatement(IEnumerable<IBoundStatement> Statements) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.BlockStatement;
}