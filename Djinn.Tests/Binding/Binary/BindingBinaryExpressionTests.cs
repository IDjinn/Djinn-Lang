using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;

namespace Djinn.Tests;

[UsesVerify]
public partial class BindingExpressionTests
{
    [Fact]
    public Task binary_expression_binding_test()
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

        return Verify(tree.Generate(binder));
    }

    [Fact]
    public Task binary_expression_binding_with_unary_expression_inside_test()
    {
        var source = """
                     function void hello(void) {
                         ret 1 + -2;
                     }
                     """;

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();

        return Verify(tree.Generate(binder));
    }
}