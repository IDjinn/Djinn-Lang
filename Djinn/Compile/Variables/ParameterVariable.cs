using Djinn.Compile.Scopes;
using Djinn.Syntax;
using LLVMSharp;

namespace Djinn.Compile.Variables;

public readonly record struct ParameterVariable(
    string Identifier,
    IScope Scope,
    dynamic ConstantValue,
    LLVMValueRef Pointer,
    LLVMValueRef FunctionPointer
    ) : IVariable
{
    public SyntaxKind Kind => SyntaxKind.ParameterVariableDeclaration;
}