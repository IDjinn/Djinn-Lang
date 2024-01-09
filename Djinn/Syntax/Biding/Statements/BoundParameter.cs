using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

public record BoundParameter(
    BoundIdentifier Type,
    BoundIdentifier Identifier,
    BoundConstantNumberLiteralExpression? ValueExpression = null
    ) :IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionParameter;
}