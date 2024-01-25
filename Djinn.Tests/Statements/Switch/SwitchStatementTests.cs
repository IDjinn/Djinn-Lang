using FluentAssertions;

namespace Djinn.Tests.Statements.Switch;

[UsesVerify]
[Collection("full-compilation-tests")]
public class SwitchStatementTests
{
    [Fact]
    public async Task test_no_default_true_branch_switch()
    {
        var source = """
                     function int1 main() {
                         switch (1) {
                            case 1: {
                                ret 1;
                            }
                        }
                        ret -2;
                     }
                     """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }


    [Fact]
    public async Task test_false_branch_switch()
    {
        var source = """
                     function int1 main() {
                         switch (2) {
                            case 1: {
                                ret -1;
                            }
                            default: {
                                ret 1;
                            }
                        }
                        ret -2;
                     }
                     """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_no_default_false_branch_switch()
    {
        var source = """
                     function int1 main() {
                         switch (2) {
                            case 1: {
                                ret -1;
                            }
                        }
                        ret 1;
                     }
                     """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_true_branch_switch()
    {
        var source = """
                     function int1 main() {
                         switch (1) {
                            case 1: {
                                ret 1;
                            }
                            default: {
                                ret -1;
                            }
                        }
                        ret -2;
                     }
                     """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}