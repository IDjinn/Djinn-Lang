using System.Diagnostics;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

[DebuggerDisplay(
    "Unary => ({OperandExpression}) => {Operator.OperatorKind.UnaryOperatorKindToString()} returns {Type.GetType().Name}")]
public record BoundUnaryExpression : IBoundExpression
{
    public required BoundUnaryOperator? Operator { get; init; }
    public required IBoundExpression OperandExpression { get; init; }
    public BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    public IType Type => OperandExpression.Type;

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        if (Operator is not null && OperandExpression is BoundConstantNumberLiteralExpression boundLiteralExpression)
        {
            var newValue = Operator.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => +(Integer32)boundLiteralExpression.Number,
                BoundUnaryOperatorKind.Negation => -(Integer32)boundLiteralExpression.Number,
                _ => throw new NotImplementedException()
            };
            var result = boundLiteralExpression with { Number = newValue };
            return result.Evaluate(expressionGenerator, boundScope);
        }

        return expressionGenerator.GenerateUnaryExpression(this, boundScope);
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        if (Operator is not null && OperandExpression is BoundConstantNumberLiteralExpression boundLiteralExpression)
        {
            var newValue = Operator.OperatorKind switch
            {
                BoundUnaryOperatorKind.Identity => +(Integer32)boundLiteralExpression.Number,
                BoundUnaryOperatorKind.Negation => -(Integer32)boundLiteralExpression.Number,
                _ => throw new NotImplementedException()
            };
            var result = boundLiteralExpression with { Number = newValue };
            return result.Generate(ctx);
        }

        var left = OperandExpression.Generate(ctx);
        return Operator?.OperatorKind switch
        {
            BoundUnaryOperatorKind.Addition => LLVM.BuildAdd(ctx.Builder, left,
                Integer32.GenerateFromValue(new Integer32(1)), "add"),
            _ => throw new NotImplementedException()
        };
    }
}