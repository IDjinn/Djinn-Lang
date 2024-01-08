using Djinn.Compile.Types;
using Djinn.Compile.Variables;
using LLVMSharp;
using Microsoft.CodeAnalysis;

namespace Djinn.Compile.Scopes;

public interface IScope
{
    public IScope? Parent { get; }
    public IReadOnlyDictionary<string, IVariable> Variables { get; }
    public IReadOnlyDictionary<string, CompilationType> Types { get; }
    public IReadOnlyDictionary<string, LLVMValueRef> Functions { get; }

    public TVariable? FindVariable<TVariable>(string identifier) where TVariable : IVariable;
    public Optional<LLVMValueRef> FindFunction(string identifier);
    public void TryCreateVariable(IVariable variable);
    public void TryCreateFunction(string identifier, LLVMValueRef functionPointer);
}