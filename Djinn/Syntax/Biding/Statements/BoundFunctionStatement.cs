using System.Diagnostics;

namespace Djinn.Syntax.Biding.Statements;

[DebuggerDisplay("Function => {Statement.GetType().Name}")]
public record BoundFunctionStatement(
    IBoundStatement Statement
) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionStatement;
}