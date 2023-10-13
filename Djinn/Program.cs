using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Statements;
using Djinn.Syntax.Biding;
using Djinn.Syntax.Biding.Statements;

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

        var source = $$"""
                       function void hello(int a) {
                           ret 1 + 2;
                       }
                       """;

// var source = $$"""1+2+3+4""";

// var source = "";
        System.Diagnostics.Debugger.Launch();
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();

        var some = (FunctionDeclarationStatement)tree.Statements.First();
        var a = binder.Visit(some);
        var test = (BoundFunctionStatement)a;
        var other = (BoundBlockStatement)test.Statement;
// IDjinn.Compile(source);

        return;
    }
}