using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;

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
                       function int64 hello(int64 b, int64 c) {
                          ret -10 + 2 * 5;
                       }
                       """;


        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        var binder = new Binder();
        var result = binder.Bind(tree);

        return;

        // string moduleError = "";
        // LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out moduleError);
        // Console.WriteLine(moduleError);
        //
        // string error = "";
        // LLVM.DumpModule(module);
        // LLVM.PrintModuleToFile(module, "test.ll", out error);
        // Console.WriteLine(error);
        // return;
    }
}