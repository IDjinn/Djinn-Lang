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
                            ret 1;
                         }
                         
                         ret -1;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
    [Fact]
    public async Task test_nested_true_branch()
    {
        var source = """
                     function void main(){
                         if(true) {
                            if(true) {
                                ret 1;
                            }
                            
                            ret -1;
                         }
                         
                         ret -2;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
    
    [Fact]
    public async Task test_nested_false_branch()
    {
        var source = """
                     function void main(){
                         if(true) {
                            if(false) {
                                ret -1;
                            }
                            
                            ret 1;
                         }
                         
                         ret -2;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_false_branch()
    {
        var source = """
                     function void main(){
                         if(false) {
                            ret -1;
                         }
                         
                         ret 1;
                     }
                     """;

        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}