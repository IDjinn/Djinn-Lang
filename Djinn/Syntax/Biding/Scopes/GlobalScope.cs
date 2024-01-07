using Microsoft.CodeAnalysis;

namespace Djinn.Syntax.Biding.Scopes;

public record GlobalScope(
   string Namespace,
   Optional<Scope> ParentScope
    ) : Scope(Namespace, ParentScope);