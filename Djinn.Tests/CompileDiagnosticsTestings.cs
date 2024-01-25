using Djinn.Compile;
using Djinn.Parsing;
using FluentAssertions;
using Binder = Djinn.Syntax.Biding.Binder;

namespace Djinn.Tests;

public class CompileDiagnosticsTestings : TestsUtilities
{
    [Theory]
    [InlineData("""
                function void main() {
                    ret 0;
                }
                """, 0)]
    public async Task should_just_compile_no_errors(string source, int errorLevel = 0)
    {
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();
        _ = binder.Bind(tree);

        Assert.Empty(tree.Diagnostics);
        Assert.Empty(binder.Reporter.Diagnostics);

        var compilerOptions = new Compiler.CompilerOptions(nameof(should_just_compile_no_errors),
            nameof(should_just_compile_no_errors));
        var ir = CompilerTools.GenerateIR(source);
        var compileResult = await CompilerTools.ClangCompileAsync(ir, compilerOptions);
        var ran = await CompilerTools.RunAsync(compilerOptions.OutputFileName);

        (await CompilerTools.RunErrorLevelCommand(compilerOptions.OutputFileName)).Should().Be(errorLevel);
    }
}