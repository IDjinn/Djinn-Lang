using System.Diagnostics.CodeAnalysis;

namespace Djinn.Utils;

public readonly record struct Optional<T>
{
    private Optional(T value)
    {
        Value = value;
        HasValue = true;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }

    public T? Value { get; init; }

    public static Optional<T> Create(T value)
    {
        return new Optional<T>(value);
    }

    public static implicit operator bool(Optional<T> optional)
    {
        return optional.HasValue;
    }
}