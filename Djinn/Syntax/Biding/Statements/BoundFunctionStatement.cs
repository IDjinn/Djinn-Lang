namespace Djinn.Syntax.Biding.Statements;

public record BoundFunctionStatement(
    IBoundStatement Statement
) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionStatement;
}