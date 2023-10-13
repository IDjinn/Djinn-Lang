using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Statements;
using Djinn.Syntax.Biding;

namespace Djinn.Tests;

public class UtilsTests
{
    [Fact]
    public void a()
    {
        var source = $$"""
                       function void hello(int a) {
                           ret 1 + a;
                       }
                       """;

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();

        var a = binder.Visit((FunctionDeclarationStatement)tree.Statements.First());
    }
}