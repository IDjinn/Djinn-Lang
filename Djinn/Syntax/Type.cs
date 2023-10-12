namespace Djinn.Syntax;

public interface IType
{
    public int SizeOf { get; }
    public bool IsHeap { get; }
    public bool IsReadOnly { get; }
}

public interface INumber : IType
{
    public bool IsFloat { get; }
    public bool Fits(string value);
}

public interface IArray<out T>
{
    public int SizeOf { get; }

    public T[] Values { get; }
}

public readonly record struct String : IType, IArray<char>
{
    public String(string value)
    {
        Values = value.ToArray();
        SizeOf = value.Length;
    }

    public char[] Values { get; }
    public int SizeOf { get; }
    public bool IsHeap => true;
    public bool IsReadOnly => true;
}

public readonly record struct Integer1(byte Value) : INumber
{
    public bool Fits(string value) => byte.TryParse(value, out _);

    public bool IsFloat => false;
    public int SizeOf => sizeof(bool);
    public bool IsHeap => false;
    public bool IsReadOnly => false;
}

public readonly record struct Integer32(int Value) : INumber
{
    public bool Fits(string value) => int.TryParse(value, out _);

    public bool IsFloat => false;
    public int SizeOf => sizeof(int);
    public bool IsHeap => false;
    public bool IsReadOnly => false;
}