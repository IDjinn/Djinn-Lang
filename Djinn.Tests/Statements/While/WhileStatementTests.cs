﻿using Djinn.Tools;
using FluentAssertions;

namespace Djinn.Tests.Statements.While;

[UsesVerify]
public class WhileStatementTests
{
    [Fact]
    public async Task test_while_true_counter_less_than()
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

    [Fact]
    public async Task? test_while_true_counter_less_than_equals()
    {
        var source = $$"""
                       function int1 main() {
                           int32 counter = 0;
                           while(counter <= 10){
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

    [Fact]
    public async Task test_while_true_counter_greater_than()
    {
        var source = $$"""
                       function int1 main() {
                           int32 counter = 10;
                           while(counter > 0){
                            printf(".");
                            counter--;
                           }
                           ret 1;
                       }
                       """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }

    [Fact]
    public async Task test_while_true_counter_greater_equals_than()
    {
        var source = $$"""
                       function int1 main() {
                           int32 counter = 10;
                           while(counter >= 0){
                            printf(".");
                            counter--;
                           }
                           ret 1;
                       }
                       """;
        var compileResult = await CompilerTools.CompileAndRun(source);
        compileResult.ErrorLevel.Should().Be(1);
        await Verify(compileResult.IR);
    }
}