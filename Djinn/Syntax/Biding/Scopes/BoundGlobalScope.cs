using Djinn.Syntax.Biding.Scopes.Types;
using Microsoft.CodeAnalysis;

namespace Djinn.Syntax.Biding.Scopes;

public record BoundGlobalScope(
    string Namespace,
    Optional<BoundScope> ParentScope = default
) : BoundScope(Namespace, ParentScope)
{
    public void Init()
    {
        CreateType(new BoundType("int32", new Integer32()));
    }
}