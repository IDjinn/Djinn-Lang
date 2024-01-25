using FluentAssertions;

namespace Djinn.Tests.Expressions.VariableManipulationExpressions;

[UsesVerify]
[Collection("full-compilation-tests")]
public class VariableManipulationExpressions
{
    [Fact]
    public async Task test_variable_default_value()
    {
        var source = """
                     function void main(){
                        int32 a;
                        a++;
                        ret a;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_variable_write()
    {
        // TODO: THIS CODE PRODUCES TWO READS. FIX ME
        var source = """
                     function void main(){
                        int32 a = 0;
                        a = 1;
                        ret a;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_variable_read()
    {
        var source = """
                     function void main(){
                        int32 a = 1;
                        ret a;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_variable_increment()
    {
        var source = """
                     function void main(){
                        int32 a = 0;
                        a++;
                        ret a;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_variable_decrement()
    {
        var source = """
                     function void main(){
                        int32 a = 2;
                        a--;
                        ret a;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}