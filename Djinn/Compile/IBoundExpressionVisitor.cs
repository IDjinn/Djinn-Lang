using Djinn.Syntax.Biding.Expressions;
using LLVMSharp;

namespace Djinn.Compile;

public interface IBoundExpressionVisitor
{
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }
    public LLVMValueRef Visit(IBoundExpression boundExpression);
    public LLVMValueRef Visit(BoundBinaryExpression boundBinaryExpression);
    public LLVMValueRef Visit(BoundUnaryExpression boundUnaryExpression);
    public LLVMValueRef Visit(BoundLiteralExpression boundLiteralExpression);
}