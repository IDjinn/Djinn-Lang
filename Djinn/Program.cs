using System.Runtime.InteropServices;
using Djinn.Compile;

namespace Djinn;

public static class Program
{
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int printf(string format, params object[] args);


    public static async Task Main(string[] args)
    {
        var source = $$"""
                       function int1 main() {
                            if (true) {
                                printf("true hit");
                            }
                            ret 1;
                       }
                       """;

        // await CompilerTools.CompileAsync(source);
        // return;

        var options = new Compiler.CompilerOptions("test", "test");
        var result = Compiler.Compile(source, options);
        Compiler.WriteToFile(result, options);
    }
}