namespace Djinn.Syntax.Biding.Scopes.Variables;

public readonly record struct BoundVariable(
    string Identifier,
    IType Type,
    BoundScope BoundScope
);