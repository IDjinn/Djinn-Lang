using Djinn.Tools;
using FluentAssertions;

namespace Djinn.Tests.Expressions;

[UsesVerify]
public class CompileTimeExpressions
{
    [Fact]
    public async Task test_unary_plus_on_constant_integers()
    {
        var source = """
                     function void main(){
                        ret 1 + 2 + 3 + 4;
                     }
                     """;
        
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(10);
        await Verify(compileResult.IR);
    }
    [Fact]
    public async Task test_unary_minus_on_constant_integers()
    {
        var source = """
                     function void main(){
                        ret 1 - 2 - 3 - 4;
                     }
                     """;
        
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(-8);
        await Verify(compileResult.IR);
    }
    
    
    [Fact]
    public async Task test_unary_mixed_on_constant_integers()
    {
        var source = """
                     function void main(){
                        ret 1 - 2 * 3 - 4;
                     }
                     """;
        
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(-9);
        await Verify(compileResult.IR);
    }
}