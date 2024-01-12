using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;

namespace Djinn.Tests;

public class CompileDiagnosticsTestings
{
    [Theory]
    [InlineData("""
                function void hello() {
                    ret printf("Hello World!");
                }
                """)]
    public async Task should_just_compile_no_errors(string source)
    {
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();
        _ = binder.Bind(tree);

        Assert.Empty(tree.Diagnostics);
        Assert.Empty(binder.Reporter.Diagnostics);

        await CompilerUtils.CompileAsync(source);
    }
}