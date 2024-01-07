using System.Diagnostics;

namespace Djinn.Syntax.Biding;

[DebuggerDisplay("{Value}")]
public record BoundValue
{
    public dynamic Value { get; init; }
    public IType Type { get; init; }
}