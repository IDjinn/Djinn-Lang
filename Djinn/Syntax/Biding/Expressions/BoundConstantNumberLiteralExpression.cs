using System.Diagnostics;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Literal => '{Number.Value}' of type '{Type.GetType().Name}'")]
public record BoundConstantNumberLiteralExpression(
    INumber Number
    ) : IBoundExpression 
{
    public BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    public IType Type => Number;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        return Number switch
        {
            Integer32 integer32 => Integer32.GenerateFromValue(integer32),
            Integer1 integer1 => Integer1.GenerateFromValue(integer1),
            _ => throw new NotImplementedException(Number.GetType().FullName)
        };
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        return Number switch
        {
            Integer32 integer32 => Integer32.GenerateFromValue(integer32),
            Integer1 integer1 => Integer1.GenerateFromValue(integer1),
            _ => throw new NotImplementedException(Number.GetType().FullName)
        };
    }
}