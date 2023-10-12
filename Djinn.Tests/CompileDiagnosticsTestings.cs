namespace Djinn.Tests;

public class CompileDiagnosticsTestings
{
    [Theory]
    [InlineData("""
                function void hello(void) {
                    ret printf("Hello World!");
                }
                """)]
    public void should_just_compile_no_errors(string source)
    {
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        Assert.Empty(tree.Diagnostics);
    }
}