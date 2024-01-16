using Djinn.Tools;
using FluentAssertions;

namespace Djinn.Tests.Statements.While;

[UsesVerify]
public class WhileStatementTests
{
    [Fact]
    public async Task test_while_true()
    {
        var source = $$"""
                       function int1 main() {
                           while(true){
                            printf(".");
                           }
                       }
                       """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(0);
        await Verify(compileResult.IR);
    }
}