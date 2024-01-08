using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

public interface IBoundExpression : IBoundNode
{
    public IType Type { get; }

    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope);
    public LLVMValueRef Generate(CompilationContext ctx);
}