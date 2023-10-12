namespace Djinn.Syntax.Biding.Statements;

public record BoundFunctionStatement(
    IBoundExpression Expression
) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionStatement;
}