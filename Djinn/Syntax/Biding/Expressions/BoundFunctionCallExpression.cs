using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

public class BoundFunctionCallExpression(
    BoundIdentifier TargetFunction,
    IEnumerable<IBoundExpression> Arguments
)
    : IBoundExpression
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionCall;

    public required IType Type { get; init; }

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        var function = ctx.Scope.FindFunction(TargetFunction.Name);
        var args = Arguments.ToList();
        var argsV = new LLVMValueRef[Math.Max(args.Count, 1)];
        for (var i = 0; i < args.Count; ++i)
        {
            argsV[i] = args[i].Generate(ctx);
        }

        return LLVM.BuildCall(ctx.Builder, function.Value, argsV, TargetFunction.Name + "_call");
    }
}