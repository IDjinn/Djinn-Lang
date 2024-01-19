using Djinn.Tools;
using FluentAssertions;

namespace Djinn.Tests.Statements.Variables;

[UsesVerify]
[Collection("full-compilation-tests")]
public class LocalVariableDeclarationTests
{
    [Fact]
    public async Task test_local_var_assignment_and_read()
    {
        var source = $$"""
                       function int1 main() {
                            int32 storage = 2;
                            ret storage;
                       }
                       """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(2);
        await Verify(compileResult.IR);
    }


    [Fact]
    public async Task test_binary_var_expression()
    {
        var source = $$"""
                       function int1 main() {
                            int32 storage = 2;
                            ret storage + 2;
                       }
                       """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(4);
        await Verify(compileResult.IR);
    }
}