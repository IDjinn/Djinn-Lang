using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes.Types;

namespace Djinn.Syntax.Biding.Statements;

public record BoundVariableStatement(
    BoundType Type,
    string Name,
    IBoundExpression Value
    ) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.DeclareVariable;
}