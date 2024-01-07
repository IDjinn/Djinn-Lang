using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Compile;

public interface IBoundExpressionGenerator
{
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }
    public LLVMValueRef Generate(IBoundExpression boundExpression, Scope scope);
    public LLVMValueRef GenerateBinaryExpression(BoundBinaryExpression boundBinaryExpression, Scope scope);
    public LLVMValueRef GenerateUnaryExpression(BoundUnaryExpression boundUnaryExpression, Scope scope);
    public LLVMValueRef GenerateLiteralExpression(BoundLiteralExpression boundLiteralExpression, Scope scope);
}