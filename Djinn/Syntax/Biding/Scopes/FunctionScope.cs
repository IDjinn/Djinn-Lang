using System.Collections.Concurrent;
using Djinn.Syntax.Biding.Statements;
using Microsoft.CodeAnalysis;

namespace Djinn.Syntax.Biding.Scopes;

public record FunctionScope(
    string Namespace,
    Optional<Scope> ParentScope
) : Scope(Namespace, ParentScope)
{
    private readonly IDictionary<string, BoundParameter> _parameters =
        new ConcurrentDictionary<string, BoundParameter>();

    public IReadOnlyDictionary<string, BoundParameter> Parameters => _parameters.AsReadOnly();

    public bool TryAddParameter(BoundParameter parameter)
    {
        if (_parameters.TryAdd(parameter.Identifier.Name, parameter))
        {
            CreateVariable(parameter.Identifier);
            return true;
        }

        return false;
    }
}