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
                           int32 counter = 0;
                           while(counter < 10){
                            printf(".");
                            counter++;
                           }
                           ret 1;
                       }
                       """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}