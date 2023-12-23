using System.Diagnostics;
using Djinn.Compile;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Literal => '{Value.Value}' of type '{Type.GetType().Name}'")]
public record BoundLiteralExpression : IBoundExpression
{
    public required BoundValue Value { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public IType Type => Value.Type;

    public LLVMValueRef Evaluate(IBoundExpressionVisitor expressionVisitor)
    {
        return Type switch
        {
            String str => String.FromValue("a", Value, expressionVisitor.Builder),
            Integer32 integer => Integer32.FromValue("a", Value, expressionVisitor.Builder),
        };
    }
}