namespace Djinn.Syntax.Biding;

public record BoundValue
{
    public object Value { get; init; }
    public Type Type { get; init; }
}