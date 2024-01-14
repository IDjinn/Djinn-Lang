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
        LLVM.FindFunction(ctx.ExecutionEngine, TargetFunction.Name, out var functionPointer);
        if (functionPointer.Pointer == IntPtr.Zero)
        {
            var function = ctx.Scope.FindFunction(TargetFunction.Name);
            functionPointer = function.HasValue ? function.Value : throw new NotImplementedException();
        }
        
        var args = Arguments.ToList();
        var argsV = new LLVMValueRef[Math.Max(args.Count, 0)];
        for (var i = 0; i < args.Count; ++i)
        {
            var argValue = args[i].Generate(ctx);
            argsV[i] = argValue; //args[i] is BoundFunctionCallExpression fnCall ? LLVM.BuildLoad(ctx.Builder, argValue, "rsfgrwf") : argValue ;
        }

        return LLVM.BuildCall(ctx.Builder, functionPointer, argsV, TargetFunction.Name + "_call");
    }
}