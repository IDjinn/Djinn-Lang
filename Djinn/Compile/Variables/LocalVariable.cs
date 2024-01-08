using Djinn.Compile.Scopes;
using Djinn.Syntax;
using LLVMSharp;

namespace Djinn.Compile.Variables;

public readonly record struct LocalVariable(
    string Identifier,
    IScope Scope,
    dynamic ConstantValue,
    LLVMValueRef Pointer,
    LLVMValueRef ScopePointer
) : IVariable
{
    public SyntaxKind Kind => SyntaxKind.LocalVariableDeclaration;
}