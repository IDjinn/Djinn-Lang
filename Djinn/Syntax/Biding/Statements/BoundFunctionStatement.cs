using System.Diagnostics;

namespace Djinn.Syntax.Biding.Statements;

[DebuggerDisplay("Function => {Statement.GetType().Name}")]
public record BoundFunctionStatement(
    BoundIdentifier Identifier,
    IEnumerable<BoundParameter> Parameters,
    IBoundStatement Statement
) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionStatement;
}