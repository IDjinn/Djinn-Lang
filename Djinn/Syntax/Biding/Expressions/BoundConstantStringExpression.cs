using System.Diagnostics;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Literal => '{Value.Value}' of type '{Type.GetType().Name}'")]
public record BoundConstantStringExpression(
    string Name,
    String Value
    ) : IBoundExpression
{
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public IType Type => Value;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        return default;
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        return String.GenerateFromValue(Name, string.Join("", Value.Values), ctx.Builder);
    }
}