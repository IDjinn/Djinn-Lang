using Djinn.Syntax.Biding.Statements;
using LLVMSharp;

namespace Djinn.Compile;

public interface IBoundStatementGenerator
{
    public LLVMValueRef Generate(BoundReturnStatement returnStatement);
    public LLVMValueRef Generate(BoundBlockStatement blockStatement);
    public LLVMValueRef Generate(BoundFunctionStatement functionStatement);

    public LLVMValueRef Generate(IBoundStatement statement);
}