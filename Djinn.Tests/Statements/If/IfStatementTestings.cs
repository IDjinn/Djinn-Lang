using Djinn.Tools;
using FluentAssertions;

namespace Djinn.Tests;

[UsesVerify]
public class IfStatementTestings
{
    [Fact]
    public async Task test_true_branch()
    {
        var source = """
                     function void main(){
                         if(true) {
                            ret 3;
                         }
                         
                         ret 1;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(3);
        await Verify(compileResult.IR);
    }
    
    [Fact]
    public async Task test_false_branch()
    {
        var source = """
                     function void main(){
                         if(false) {
                            ret 3;
                         }
                         
                         ret 1;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}