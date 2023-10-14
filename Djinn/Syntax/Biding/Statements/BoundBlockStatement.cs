using System.Diagnostics;

namespace Djinn.Syntax.Biding.Statements;

[DebuggerDisplay("Block({count}) => [{values}]")]
public record BoundBlockStatement(IEnumerable<IBoundStatement> Statements) : IBoundStatement
{
    public static BoundBlockStatement Empty => new BoundBlockStatement(new IBoundStatement[] { });
    public BoundNodeKind Kind => BoundNodeKind.BlockStatement;
#if DEBUG
    public int count = Statements.Count();
    public string values = string.Join(", ", Statements.Select(x => x.GetType().Name));
#endif
}