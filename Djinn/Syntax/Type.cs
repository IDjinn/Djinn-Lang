using Djinn.Syntax.Biding;
using LLVMSharp;

namespace Djinn.Syntax;

public interface IType
{
    public int SizeOf { get; }
    public bool IsHeap { get; }
    public bool IsReadOnly { get; }
}

public readonly record struct Bool(bool Value) : IType
{
    public int SizeOf => 1;
    public bool IsHeap => false;
    public bool IsReadOnly => false;
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

    public static LLVMValueRef FromValue(string name, BoundValue value, LLVMBuilderRef builderRef)
    {
        return LLVM.BuildGlobalString(builderRef, value.Value.ToString(), name);
    }
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

    public static LLVMValueRef FromValue(string name, BoundValue value, LLVMBuilderRef builderRef)
    {
        var integer = (Integer32)value.Value;
        return LLVM.ConstInt(LLVMTypeRef.Int32Type(), (ulong)integer.Value, new LLVMBool(0));
    }
}