using System.Collections.Concurrent;
using Djinn.Compile.Types;
using Djinn.Compile.Variables;
using LLVMSharp;
using Microsoft.CodeAnalysis;

namespace Djinn.Compile.Scopes;

public record CompilationScope(
    string MangledName,
    IScope? Parent = null
) : IScope
{
    private readonly IDictionary<string, LLVMValueRef> _functions = new ConcurrentDictionary<string, LLVMValueRef>();

    private readonly IDictionary<string, CompilationType> _types =
        new ConcurrentDictionary<string, CompilationType>(Compiler.STD.Types);

    private readonly IDictionary<string, IVariable> _variables = new ConcurrentDictionary<string, IVariable>();

    public IReadOnlyDictionary<string, IVariable> Variables => _variables.AsReadOnly();
    public IReadOnlyDictionary<string, CompilationType> Types => _types.AsReadOnly();
    public IReadOnlyDictionary<string, LLVMValueRef> Functions => _functions.AsReadOnly();

    public Optional<TVariable> FindVariable<TVariable>(string identifier) where TVariable : IVariable
    {
        if (_variables.TryGetValue(identifier, out var variable))
            return variable is TVariable tVar ? tVar : default(Optional<TVariable>);

        return default;
    }

    public Optional<LLVMValueRef> FindFunction(string identifier)
    {
        return _functions.TryGetValue(identifier, out var function)
            ? function
            : Parent?.FindFunction(identifier) ?? default;
    }

    public void TryCreateVariable(IVariable variable)
    {
        Parent?.TryCreateVariable(variable);
        _variables.TryAdd(variable.Identifier, variable);
    }

    public void TryCreateFunction(string identifier, LLVMValueRef functionPointer)
    {
        Parent?.TryCreateFunction(identifier, functionPointer);
        _functions.Add(identifier, functionPointer);
    }
}