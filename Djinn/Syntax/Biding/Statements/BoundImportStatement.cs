namespace Djinn.Syntax.Biding.Statements;

public record BoundImportStatement(
    string TargetLibrary 
    ) : IBoundStatement
{ 
    public BoundNodeKind Kind => BoundNodeKind.Import;
}