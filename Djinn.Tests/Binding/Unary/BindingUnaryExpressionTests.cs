using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;

namespace Djinn.Tests;

public partial class BindingExpressionTests
{
    [Fact]
    public Task unary_expression_binding_test()
    {
        var source = """
                     function void hello(void) {
                         ret +1;
                     }
                     """;

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();

        return Verify(tree.Generate(binder));
    }
}