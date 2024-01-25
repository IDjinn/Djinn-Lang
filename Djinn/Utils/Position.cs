namespace Djinn.Utils;

public readonly record struct Position(
    int Index,
    int Lenght,
    int Ident = 0,
    int Line = 0,
    int Column = 0
);