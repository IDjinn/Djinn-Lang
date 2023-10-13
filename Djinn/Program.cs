using Djinn.Compile;
using Djinn.Lexing;
using Djinn.Parsing;
using LLVMSharp;

namespace Djinn;

public static class Program
{
    public static void Main(string[] args)
    {
// var source = $$"""
//                function void hello(void) {
//                    ret printf("Hello World!");
//                }
//                """;
//
//         var source = $$"""
//                        function void hello(int a) {
//                            ret 1 + 2;
//                        }
//                        """;

// var source = $$"""1+2+3+4""";

        var source = $$"""
                       function void main() {
                          hello(1,2);
                       }
                       function int64 hello(int b, int64 c) {
                          ret 10;
                       }
                       """;


        System.Diagnostics.Debugger.Launch();
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        var codegen = new CodeGen(tree);
        var module = codegen.GenerateLlvm();
        string moduleError = "";
        LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out moduleError);
        Console.WriteLine(moduleError);

        string error = "";
        LLVM.DumpModule(module);
        LLVM.PrintModuleToFile(module, "test.ll", out error);
        Console.WriteLine(error);
        return;
    }
}