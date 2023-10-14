using System.Diagnostics;
using Djinn.Syntax.Biding.Expressions;

namespace Djinn.Syntax.Biding.Statements;

[DebuggerDisplay("Function => {Expression.GetType().Name}")]
public record BoundReturnStatement(IBoundExpression Expression) : IBoundStatement
{
    public BoundNodeKind Kind => BoundNodeKind.ReturnStatement;
}