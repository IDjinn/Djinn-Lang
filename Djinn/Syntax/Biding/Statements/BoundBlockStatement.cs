namespace Djinn.Syntax.Biding.Statements;

public record BoundBlockStatement(IEnumerable<IBoundStatement> Statements) : IBoundStatement
{
    public static BoundBlockStatement Empty => new BoundBlockStatement(new IBoundStatement[] { });
    public BoundNodeKind Kind => BoundNodeKind.BlockStatement;
}