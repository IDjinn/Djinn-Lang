using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Compile;

public interface IBoundExpressionGenerator
{
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }
    public LLVMModuleRef Module { get; init; }
    public LLVMValueRef Generate(IBoundExpression boundExpression, BoundScope boundScope);
    public LLVMValueRef GenerateBinaryExpression(BoundBinaryExpression boundBinaryExpression, BoundScope boundScope);
    public LLVMValueRef GenerateUnaryExpression(BoundUnaryExpression boundUnaryExpression, BoundScope boundScope);
    public LLVMValueRef GenerateLiteralExpression(BoundLiteralExpression boundLiteralExpression, BoundScope boundScope);
}