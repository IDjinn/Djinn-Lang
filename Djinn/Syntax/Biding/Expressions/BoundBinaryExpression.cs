using System.Diagnostics;
using Djinn.Compile;
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

    public LLVMValueRef Evaluate(IBoundExpressionVisitor expressionVisitor)
    {
        var left = expressionVisitor.Visit(Left);
        var right = expressionVisitor.Visit(Right);

        return Operator.OperatorKind switch
        {
            BoundBinaryOperatorKind.Addition => LLVM.BuildAdd(expressionVisitor.Builder, left, right, "plus"),
            BoundBinaryOperatorKind.Subtraction => LLVM.BuildSub(expressionVisitor.Builder, left, right, "sub"),
            BoundBinaryOperatorKind.Division => LLVM.BuildFDiv(expressionVisitor.Builder, left, right, "div"),
            BoundBinaryOperatorKind.Multiplication => LLVM.BuildMul(expressionVisitor.Builder, left, right, "mult"),
            _ => throw new InvalidOperationException("Invalid operation")
        };
    }
}