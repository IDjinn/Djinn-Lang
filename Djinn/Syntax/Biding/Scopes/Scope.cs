using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Djinn.Syntax.Biding.Scopes;

[DebuggerDisplay("Scope=[{Namespace}]")]
public record Scope
{

public string Namespace { get; private init; }
public Optional<Scope> ParentScope { get; private init; }

public Scope(string nameSpace, Optional<Scope> parentScope = default)
{
    Namespace= GenerateUniqueScopeId(this, nameSpace);
    ParentScope = parentScope;
}


    private IDictionary<string, BoundIdentifier> _variables = new ConcurrentDictionary<string, BoundIdentifier>();
    public ReadOnlyDictionary<string, BoundIdentifier> Variables => _variables.AsReadOnly();


    public void CreateVariable(BoundIdentifier identifier)
    {
        _variables.Add(identifier.Name, identifier);
    }

    public Optional<BoundIdentifier> FindVariable(string identifier)
    {
        if (_variables.TryGetValue(identifier, out var variable)) return variable;

        return ParentScope.HasValue ? ParentScope.Value.FindVariable(identifier) : default;
    }
    
    public static string GenerateUniqueScopeId(Scope scope, string @namespace)
    {
        return $"__{scope.GetType().Name}_{@namespace}";
    }
};