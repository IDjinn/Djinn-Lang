using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Statements;
using LLVMSharp;

namespace Djinn.Compile;

public interface IBoundStatementGenerator
{
    public LLVMValueRef GenerateReturnStatement(BoundReturnStatement returnStatement, BoundScope boundScope);
    public LLVMValueRef GenerateBlockStatement(BoundBlockStatement blockStatement, BoundScope boundScope);
    public LLVMValueRef GenerateFunctionStatement(BoundFunctionStatement functionStatement, BoundFunctionScope scope);
    public LLVMValueRef GenerateStatement(IBoundStatement statement, BoundScope boundScope);
}