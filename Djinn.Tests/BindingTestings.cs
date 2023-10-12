using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;

namespace Djinn.Tests;

public class BindingTestings
{
    [Fact]
    public void unary_expression_binding_test()
    {
        var source = """
                     function void hello(void) {
                         ret 1 + 2;
                     }
                     """;

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();

        var a = tree.Generate(binder);
    }
}