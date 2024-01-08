using System.Collections.Concurrent;
using Djinn.Syntax.Biding.Scopes.Types;
using Djinn.Syntax.Biding.Scopes.Variables;
using Djinn.Syntax.Biding.Statements;
using Microsoft.CodeAnalysis;

namespace Djinn.Syntax.Biding.Scopes;

public record BoundFunctionScope(
    string Namespace,
    Optional<BoundScope> ParentScope
) : BoundScope(Namespace, ParentScope)
{
    private readonly IDictionary<string, BoundVariable> _parameters =
        new ConcurrentDictionary<string, BoundVariable>();

    public IReadOnlyDictionary<string, BoundVariable> Parameters => _parameters.AsReadOnly();

    public override Optional<BoundType> FindType(string identifier)
    {
        return base.FindType(identifier);
    }
    

    public override Optional<BoundVariable> FindVariable(string identifier)
    {
        if (_parameters.TryGetValue(identifier, out var param)) return param;
        
        return base.FindVariable(identifier);
    }

    public void AddParameter(BoundVariable parameter)
    {
        _parameters.TryAdd(parameter.Identifier, parameter);
        CreateVariable(parameter);
    }
}