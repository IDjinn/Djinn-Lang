using Djinn.Syntax.Biding;
using LLVMSharp;

namespace Djinn.Syntax;

public interface IType
{
    public int SizeOf { get; }
    public bool IsHeap { get; }
    public bool IsReadOnly { get; }

    public LLVMTypeRef ToLLVMType();
}

public readonly record struct Bool(bool Value) : IType
{
    public int SizeOf => 1;
    public bool IsHeap => false;
    public bool IsReadOnly => false;

    public LLVMTypeRef ToLLVMType()
    {
        return LLVM.Int1Type();
    }

    public static LLVMValueRef GenerateFromValue(Bool boolean)
    {
        return LLVM.ConstInt(LLVMTypeRef.Int1Type(), (ulong)(boolean.Value ? 1 : 0), new LLVMBool(0));
    }
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

    public LLVMTypeRef ToLLVMType()
    {
        throw new NotImplementedException();
    }

    public static LLVMValueRef GenerateFromValue(string name, BoundValue value, LLVMBuilderRef builderRef)
    {
        return LLVM.BuildGlobalString(builderRef, value.Value.ToString(), name);
    }

    public static LLVMValueRef GenerateFromValue(string name, string rawString, LLVMBuilderRef builderRef)
    {
        return LLVM.BuildGlobalString(builderRef, rawString, name);
    }
}

public readonly record struct Integer1(byte Value) : INumber
{
    public bool Fits(string value) => byte.TryParse(value, out _);
    public bool IsFloat => false;
    public int SizeOf => sizeof(bool);
    public bool IsHeap => false;
    public bool IsReadOnly => false;

    public LLVMTypeRef ToLLVMType()
    {
        return LLVM.Int1Type();
    }

    public static LLVMValueRef GenerateFromValue(Integer1 integer)
    {
        return LLVM.ConstInt(LLVMTypeRef.Int1Type(), integer.Value, new LLVMBool(0));
    }

    public static Integer1 operator +(Integer1 a, Integer1 b)
    {
        return new Integer1((byte)(a.Value + b.Value));
    }

    public static Integer1 operator -(Integer1 a, Integer1 b)
    {
        return new Integer1((byte)(a.Value - b.Value));
    }

    public static Integer1 operator *(Integer1 a, Integer1 b)
    {
        return new Integer1((byte)(a.Value * b.Value));
    }

    public static Integer1 operator /(Integer1 a, Integer1 b)
    {
        return new Integer1((byte)(a.Value / b.Value));
    }

    public static Integer1 operator +(Integer1 a)
    {
        return new Integer1((byte)+a.Value);
    }

    public static Integer1 operator -(Integer1 a)
    {
        return new Integer1((byte)-a.Value);
    }

    public static explicit operator Integer1(byte value)
    {
        return new Integer1(value);
    }
}

public readonly record struct Integer32(int Value) : INumber
{
    public bool Fits(string value) => int.TryParse(value, out _);

    public bool IsFloat => false;
    public int SizeOf => sizeof(int);
    public bool IsHeap => false;
    public bool IsReadOnly => false;

    public LLVMTypeRef ToLLVMType()
    {
        return LLVM.Int32Type();
    }

    public static LLVMValueRef GenerateFromValue(Integer32 integer32)
    {
        return LLVM.ConstInt(LLVMTypeRef.Int32Type(), (ulong)integer32.Value, new LLVMBool(0));
    }

    public static Integer32 operator +(Integer32 a, Integer32 b)
    {
        return new Integer32(a.Value + b.Value);
    }

    public static Integer32 operator -(Integer32 a, Integer32 b)
    {
        return new Integer32(a.Value - b.Value);
    }

    public static Integer32 operator *(Integer32 a, Integer32 b)
    {
        return new Integer32(a.Value * b.Value);
    }

    public static Integer32 operator /(Integer32 a, Integer32 b)
    {
        return new Integer32(a.Value / b.Value);
    }

    public static Integer32 operator +(Integer32 a)
    {
        return new Integer32(+a.Value);
    }

    public static Integer32 operator -(Integer32 a)
    {
        return new Integer32(-a.Value);
    }
}