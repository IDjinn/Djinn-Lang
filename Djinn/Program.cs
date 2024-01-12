using System.Runtime.InteropServices;
using Djinn.Compile;
using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;
using LLVMSharp;

namespace Djinn;

public static class Program
{
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int printf(string format, params object[] args);


    public static void Main(string[] args)
    {

        var source = $$"""
                       function int1 main() {
                            if (true) {
                                ret 2;
                            }
                            ret 1;
                       }
                       """;


        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        if (parser.Diagnostics.Any())
        {
            throw new Exception();
        }

        var binder = new Binder();
        var bindResult = binder.Bind(tree);
        if (binder.Reporter.Diagnostics.Any())
        {
            throw new Exception();
        }
        Compiler.Compile(bindResult);
        return;
        
        var compiler = new LLVMCompiler(bindResult);
        compiler.Compile();

        string moduleError = "";
        LLVM.VerifyModule(compiler.Module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out moduleError);
        Console.WriteLine(moduleError);

        string error = "";
        LLVM.DumpModule(compiler.Module);
        LLVM.PrintModuleToFile(compiler.Module, "test.ll", out error);
        Console.WriteLine(error);
        return;
    }
}