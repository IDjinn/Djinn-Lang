using System.Collections.Concurrent;
using Djinn.Compile.Types;
using Djinn.Compile.Variables;
using LLVMSharp;
using Microsoft.CodeAnalysis;

namespace Djinn.Compile.Scopes;

public record CompilationFunctionScope(
    string MangledName,
    IScope? Parent
    ) : CompilationScope(MangledName, Parent)
{
    public IReadOnlyDictionary<string, ParameterVariable> Parameters => _parameters.AsReadOnly();

    private readonly IDictionary<string, ParameterVariable> _parameters =
        new ConcurrentDictionary<string, ParameterVariable>();

    public void AddParameter(ParameterVariable parameter)
    {
        _parameters.Add(parameter.Identifier, parameter);
        TryCreateVariable(parameter);
    }

    public Optional<ParameterVariable> GetParameter(string identifier) =>
        _parameters.TryGetValue(identifier, out var parameter) ? parameter : default;
}