using LLVMSharp;

namespace Djinn.Utils;

public static class LLVMUtils
{
    public static bool IsNullPointer(this LLVMValueRef @ref) => @ref.Pointer == IntPtr.Zero;
}