using System.Diagnostics;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay("Binary => [{Left}] {Operator.OperatorKind} [{Right}] return {Type?.GetType().Name}")]
public record BoundBinaryExpression : IBoundExpression
{
    public required IBoundExpression Left { get; init; }
    public required BoundBinaryOperator Operator { get; init; }
    public required IBoundExpression Right { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    public IType Type => Operator.ResultType;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, Scope scope)
    {
        var left = expressionGenerator.Generate(Left, scope);
        var right = expressionGenerator.Generate(Right, scope);

        return Operator.OperatorKind switch
        {
            BoundBinaryOperatorKind.Addition => LLVM.BuildAdd(expressionGenerator.Builder, left, right, "plus"),
            BoundBinaryOperatorKind.Subtraction => LLVM.BuildSub(expressionGenerator.Builder, left, right, "sub"),
            BoundBinaryOperatorKind.Division => LLVM.BuildFDiv(expressionGenerator.Builder, left, right, "div"),
            BoundBinaryOperatorKind.Multiplication => LLVM.BuildMul(expressionGenerator.Builder, left, right, "mult"),
            _ => throw new InvalidOperationException("Invalid operation")
        };
    }
}