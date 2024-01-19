using Djinn.Tools;
using FluentAssertions;

namespace Djinn.Tests.Statements.For;

[UsesVerify]
[Collection("full-compilation-tests")]
public class ForStatementTestings
{
    [Fact]
    public async Task test_for_declaring_variable()
    {
        var source = $$"""
                       function int1 main() {
                            for (int32 i = 0; i < 10; i++) {
                                printf("i = %d\n", i);
                            }
                            ret 1;
                       }
                       """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}