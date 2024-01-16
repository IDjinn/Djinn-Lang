using System.Reflection.Metadata;
using Djinn.Compile;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Scopes.Variables;
using LLVMSharp;
using LocalVariable = Djinn.Compile.Variables.LocalVariable;

namespace Djinn.Syntax.Biding.Expressions;

public record BoundAssigmentVariableExpression(
    BoundVariable Variable,
    IBoundExpression Expression
    ) : IBoundExpression
{
    public BoundNodeKind Kind => BoundNodeKind.AssigmentVariable;
    public IType Type { get; }
    public LLVMValueRef Evaluate(IBoundExpressionGenerator expressionGenerator, BoundScope boundScope)
    {
        throw new NotImplementedException();
    }

    public LLVMValueRef Generate(CompilationContext ctx)
    {
        var variable = ctx.Scope.FindVariable<LocalVariable>(Variable.Identifier);
        if (!variable.HasValue)
        {
            throw new NotImplementedException();
        }
        
        var value = Expression.Generate(ctx);
        return LLVM.BuildStore(ctx.Builder, value, variable.Value.Pointer);
    }
}