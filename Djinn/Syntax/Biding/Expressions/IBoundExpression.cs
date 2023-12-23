using Djinn.Compile;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

public interface IBoundExpression : IBoundNode
{
    public IType Type { get; }

    public LLVMValueRef Evaluate(IBoundExpressionVisitor expressionVisitor);
}