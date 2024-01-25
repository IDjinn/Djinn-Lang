using FluentAssertions;

namespace Djinn.Tests.Statements.Arrow;

[UsesVerify]
[Collection("full-compilation-tests")]
public class ArrowFunctionStatement
{
    [Fact]
    public async Task test_arrow_function_statement()
    {
        var source = $$"""
                       function int1 main() => ret 2;
                       """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(2);
        await Verify(compileResult.IR);
    }
}