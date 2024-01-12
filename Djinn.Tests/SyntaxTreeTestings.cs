using Djinn.Expressions;
using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Statements;
using Djinn.Syntax;
using FluentAssertions;

namespace Djinn.Tests;

public class SyntaxTreeTestings
{
    [Fact]
    public void check_hello_world_syntax_tree()
    {
        var source = """
                     function void hello() {
                         ret printf("Hello World!");
                     }
                     """;

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        tree.Should().NotBeNull();

        tree.Diagnostics.Should().BeEmpty();
        tree.Statements.Should().NotBeEmpty();

        tree.Statements.Should().ContainSingle();/*
        var blockStatement = (BlockStatement)tree.Statements.First();
        blockStatement.Should().BeOfType<BlockStatement>();
        blockStatement.Statements.Should().ContainSingle();

        var statement = (ReturnStatement)blockStatement.Statements.First();
        statement.Should().BeOfType<ReturnStatement>();

        statement.ExpressionSyntax.Should().BeOfType<FunctionCallExpression>();*/
    }

    [Fact]
    public void check_add_function_syntax_tree()
    {
        var source = """
                     function void hello() {
                         ret 1 + 2;
                     }
                     """;

        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        tree.Should().NotBeNull();

        tree.Diagnostics.Should().BeEmpty();
        tree.Statements.Should().NotBeEmpty();

        tree.Statements.Should().ContainSingle();
        var blockStatement = (BlockStatement)tree.Statements.First();/*
        blockStatement.Should().BeOfType<BlockStatement>();
        blockStatement.Statements.Should().ContainSingle();

        var statement = (ReturnStatement)blockStatement.Statements.First();
        statement.Should().BeOfType<ReturnStatement>();

        var binaryExpression = (BinaryExpressionSyntax)statement.ExpressionSyntax;
        binaryExpression.Should().BeOfType<BinaryExpressionSyntax>();
        binaryExpression.LeftExpression.Should().BeOfType<ConstantNumberExpressionSyntax>();
        binaryExpression.Operator.Kind.Should().Be(SyntaxKind.PlusToken);
        binaryExpression.RightExpression.Should().BeOfType<ConstantNumberExpressionSyntax>();*/
    }
}