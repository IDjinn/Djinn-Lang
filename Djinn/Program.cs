using System.Runtime.InteropServices;

namespace Djinn;

public static class Program
{
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int printf(string format, params object[] args);


    public static async Task Main(string[] args)
    {
        var source = $$"""
                       function int1 main() {
                            int32 a = 0;
                            a++;
                            ret a;
                       }
                       """;

        if (!Directory.Exists("./temp"))
            Directory.CreateDirectory("./temp");

        var compileResult = await CompilerTools.CompileAndRun(source);
        Console.Clear();
        Console.WriteLine(compileResult.IR);
        Console.WriteLine($"[ERROR-LEVEL] = {compileResult.ErrorLevel}");
    }
}