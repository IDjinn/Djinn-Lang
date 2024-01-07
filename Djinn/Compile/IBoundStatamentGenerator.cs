using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Statements;
using LLVMSharp;

namespace Djinn.Compile;

public interface IBoundStatementGenerator
{
    public LLVMValueRef GenerateReturnStatement(BoundReturnStatement returnStatement, Scope scope);
    public LLVMValueRef GenerateBlockStatement(BoundBlockStatement blockStatement, Scope scope);
    public LLVMValueRef GenerateFunctionStatement(BoundFunctionStatement functionStatement, FunctionScope scope);
    public LLVMValueRef GenerateStatement(IBoundStatement statement, Scope scope);
}