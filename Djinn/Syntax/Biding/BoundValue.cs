namespace Djinn.Syntax.Biding;

public record BoundValue
{
    public object Value { get; init; }
    public IType Type { get; init; }
}