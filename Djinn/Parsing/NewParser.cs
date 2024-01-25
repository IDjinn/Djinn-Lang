using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Parsing;

public record Variable(string Name, SyntaxToken Type, object Value);

public class NewParser
{
    public readonly string Source;
    public readonly ImmutableList<SyntaxToken> Tokens;

    public NewParser(IEnumerable<SyntaxToken> tokens, string source)
    {
        Source = source;
        Tokens = tokens.ToImmutableList();
    }

    public IEnumerable<IStatement> Parse()
    {
        var statements = new List<IStatement>();
        do
        {
            var current = Current;
            if (current.Kind.HasFlag(SyntaxKind.FunctionDeclaration))
                statements.Add(ParseFunction());
            else if (current.Kind.HasFlag(SyntaxKind.Import))
                statements.Add(ParseImport());
            else
                throw new NotImplementedException();
        } while (HasCurrent && !Current.Kind.HasFlag(SyntaxKind.EndOfFileToken));

        return statements;
    }

    IStatement ParseImport()
    {
        Consume(SyntaxKind.Import);
        var library = Consume(SyntaxKind.StringLiteral);
        Consume(SyntaxKind.SemiColon);
        return new ImportStatement(library);
    }

    public FunctionDeclarationStatement ParseFunction()
    {
        Consume(SyntaxKind.FunctionDeclaration);
        var returnType = Consume(SyntaxKind.Type);
        var name = Consume(SyntaxKind.Identifier);
        var functionParameters = ParseFunctionParameters();

        var variables = new Stack<Variable>();
        var body = ParseFunctionBody();

        return new FunctionDeclarationStatement(returnType, name, functionParameters, body);

        IStatement ParseFunctionBody()
        {
            return Current.Kind switch
            {
                SyntaxKind.OpenBrace => ParseBlockStatement(),
                SyntaxKind.Arrow => ParseInlineFunctionBody(),
                _ => throw new InvalidEnumArgumentException($"Invalid kind of function body: '{Current.Kind}'")
            };

            IStatement ParseInlineFunctionBody()
            {
                Consume(SyntaxKind.Arrow);
                return new BlockStatement(new[] { ParseReturnStatement() });
            }

            IExpressionSyntax ParseExpression(int parentPrecedence = 0)
            {
                IExpressionSyntax left;
                var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
                if (!unaryOperatorPrecedence.HasValue || parentPrecedence > unaryOperatorPrecedence.Value)
                {
                    left = ParseLeftExpression();
                }
                else
                {
                    throw new NotImplementedException();
                }

                while (true)
                {
                    var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                    if (!precedence.HasValue || precedence <= parentPrecedence)
                        break;

                    var operatorToken = Consume(SyntaxKind.LogicalOperators);
                    var right = ParseExpression(precedence.Value);
                    left = new BinaryExpressionSyntax(left, operatorToken, right);
                }

                return left;
            }

            IExpressionSyntax ParseLeftExpression()
            {
                if (TryConsume(SyntaxKind.StringLiteral, out var stringToken))
                    return new ConstantStringExpressionSyntax(stringToken);
                if (TryConsume(SyntaxKind.NumberLiteral, out var numberToken))
                    return new ConstantNumberExpressionSyntax(numberToken);
                if (TryConsume(SyntaxKind.True, out var trueToken))
                    return new ConstantBooleanExpression(trueToken);
                if (TryConsume(SyntaxKind.True, out var falseToken))
                    return new ConstantBooleanExpression(falseToken);

                var identifier = ConsumeOptional(SyntaxKind.Identifier);
                if (identifier is not null)
                {
                    var keyword = KeywordExtensions.FromString((string)identifier.Value);
                    if (keyword != Keyword.Unknown)
                        throw new NotImplementedException(
                            $"Keyword '{identifier.Value}' is not valid as expression.");


                    var parenthesis = ConsumeOptional(SyntaxKind.OpenParenthesis);
                    if (parenthesis is not null)
                    {
                        var functionCallExpression =
                            new FunctionCallExpression(identifier, new List<IExpressionSyntax>());
                        Consume(SyntaxKind.CloseParenthesis);
                        return functionCallExpression; //TODO THIS;
                    }

                    return new ReadVariableExpression(identifier, identifier); // TODO variable type at parsing time
                }

                throw new InvalidOperationException($"Unsupported expression of kind '{Current.Kind}'");
            }

            IStatement ParseReturnStatement()
            {
                Consume(SyntaxKind.Return);
                IExpressionSyntax? expression = null;
                var colon = ConsumeOptional(SyntaxKind.SemiColon);
                if (colon is null)
                {
                    expression = ParseExpression();
                    Consume(SyntaxKind.SemiColon);
                }

                return new ReturnStatement(expression);
            }

            IStatement ParseStatement()
            {
                if (Current.Kind.HasFlag(SyntaxKind.Identifier) ||
                    Current.Kind.HasFlag(SyntaxKind.Type))
                    return ParseIdentifierStatement();

                return Current.Kind switch
                {
                    SyntaxKind.Return => ParseReturnStatement(),
                    SyntaxKind.If => ParseIfStatement(),
                    SyntaxKind.While => ParseWhileStatement(),
                    SyntaxKind.For => ParseForStatement(),
                    SyntaxKind.OpenBrace => ParseBlockStatement(),
                    _ => throw new NotImplementedException($"Invalid kind '{Current.Kind}'")
                };

                IStatement ParseForStatement()
                {
                    throw new NotImplementedException();
                }

                IStatement ParseWhileStatement()
                {
                    throw new NotImplementedException();
                }

                // variable++ 
                IStatement ParseVariableManipulation()
                {
                    var identifier = Consume(SyntaxKind.Identifier);
                    if (TryConsume(SyntaxKind.ArithmeticOperators, out var arithmeticOperatorToken))
                    {
                        if (arithmeticOperatorToken.Kind.HasFlag(SyntaxKind.IncrementOperator) ||
                            arithmeticOperatorToken.Kind.HasFlag(SyntaxKind.DecrementOperator))
                        {
                            var binary = new BinaryExpressionSyntax(
                                new ReadVariableExpression(identifier, identifier), // TODO: THIS SHOULD BE DYNAMIC
                                arithmeticOperatorToken,
                                new ConstantNumberExpressionSyntax(arithmeticOperatorToken)
                            );
                            Consume(SyntaxKind.SemiColon);

                            return new DiscardExpressionResultStatement(
                                new AssigmentExpression(
                                    identifier,
                                    arithmeticOperatorToken,
                                    binary)
                            );
                        }

                        throw new NotImplementedException();
                    }
                    else if (TryConsume(SyntaxKind.LogicalOperators, out var logicalOperatorToken))
                    {
                        if (logicalOperatorToken.Kind.HasFlag(SyntaxKind.EqualsOperator))
                        {
                            var localVariable = variables.FirstOrDefault(x => x.Name == (string)identifier.Value);
                            if (localVariable is null)
                                throw new InvalidOperationException($"Variable '{identifier.Value}' not found.");

                            var assignExpression = HasCurrent && Current.Kind.HasFlag(SyntaxKind.SemiColon)
                                ? VariableDeclarationStatement
                                    .ConstantDefaultValueOfType(localVariable.Type) // TODO int a=; should be illegal
                                : ParseExpression(); // TODO THIS
                            Consume(SyntaxKind.SemiColon);
                            return new DiscardExpressionResultStatement(new AssigmentExpression(
                                identifier,
                                logicalOperatorToken,
                                assignExpression
                            ));
                        }
                    }

                    throw new NotImplementedException();
                }

                IStatement ParseIdentifierStatement()
                {
                    var currentIsType = Current.Kind.HasFlag(SyntaxKind.Type);
                    var currentIsIdentifier = Current.Kind.HasFlag(SyntaxKind.Identifier);
                    var nextIsArithmetic = HasNext && Next.Kind.HasFlag(SyntaxKind.ArithmeticOperators);
                    var nextIsIdentifier = HasNext && Next.Kind.HasFlag(SyntaxKind.Identifier);
                    var nextIsLogical = HasNext && Next.Kind.HasFlag(SyntaxKind.LogicalOperators);
                    if (currentIsIdentifier && (nextIsArithmetic || nextIsLogical))
                        return ParseVariableManipulation();

                    // type identifier
                    if (currentIsType && nextIsIdentifier)
                    {
                        var variableType = Consume(SyntaxKind.Type);
                        var identifier = Consume(SyntaxKind.Identifier);
                        if (TryConsume(SyntaxKind.SemiColon, out _))
                        {
                            variables.Push(new Variable((string)identifier.Value, variableType, null));
                            return new VariableDeclarationStatement(variableType, identifier,
                                VariableDeclarationStatement.ConstantDefaultValueOfType(variableType));
                        }

                        if (TryConsume(SyntaxKind.EqualsOperator, out _)) // this is a variable declaration
                        {
                            var expression = ParseExpression();
                            Consume(SyntaxKind.SemiColon);
                            variables.Push(new Variable((string)identifier.Value, variableType, null));
                            return new VariableDeclarationStatement(variableType, identifier, expression);
                        }

                        throw new NotImplementedException();
                    }

                    throw new NotImplementedException();
                }

                IStatement ParseIfStatement()
                {
                    Consume(SyntaxKind.If);
                    Consume(SyntaxKind.OpenParenthesis);
                    var conditional = ParseExpression();
                    Consume(SyntaxKind.CloseParenthesis);

                    var ifBlock = ParseBlockStatement();
                    IStatement? statement = null;
                    if (Current.Kind.HasFlag(SyntaxKind.Else))
                    {
                        Advance();
                        statement = ParseStatement();
                    }

                    return new IfStatement(
                        conditional,
                        ifBlock,
                        statement
                    );
                }

                IStatement ParseKeywordStatement()
                {
                    throw new NotImplementedException();
                }
            }

            BlockStatement ParseBlockStatement()
            {
                var blockStatements = new List<IStatement>();
                Consume(SyntaxKind.OpenBrace);
                while (HasCurrent && !Current.Kind.HasFlag(SyntaxKind.CloseBrace))
                {
                    blockStatements.Add(ParseStatement());
                }

                Consume(SyntaxKind.CloseBrace);

                return new BlockStatement(blockStatements);
            }
        }

        IEnumerable<ParameterDeclaration> ParseFunctionParameters()
        {
            var parameters = new List<ParameterDeclaration>();
            Consume(SyntaxKind.OpenParenthesis);
            while (HasCurrent && !Current.Kind.HasFlag(SyntaxKind.CloseParenthesis))
            {
            }

            Consume(SyntaxKind.CloseParenthesis);
            return parameters;
        }

        ;
    }

    #region token control

    public int Index { get; set; }
    public bool HasCurrent => Index < Tokens.Count;

    public SyntaxToken Current => HasCurrent ? Tokens[Index] : SyntaxToken.EndOfFileToken;

    public bool HasNext => Index + 1 < Tokens.Count;

    public SyntaxToken Next => HasNext ? Tokens[Index + 1] : SyntaxToken.EndOfFileToken;
    public bool HasPrevious => Index - 1 >= 0;
    public SyntaxToken Previous => HasPrevious ? Tokens[Index - 1] : SyntaxToken.BadToken;

    public bool TryConsume(SyntaxKind kind, [NotNullWhen(true)] out SyntaxToken? token)
    {
        token = null;
        if (!Current.Kind.HasFlag(kind))
            return false;

        token = Current;
        Advance();
        return true;
    }

    public SyntaxToken Consume(SyntaxKind kind)
    {
        Debug.Assert(Current.Kind.HasFlag(kind));
        var aux = Current;
        Advance();
        return aux;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Advance(uint count = 1)
    {
        Debug.Assert(count > 0);
        Index += (int)count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Retreat(uint count = 1)
    {
        Debug.Assert(count > 0);
        Debug.Assert(HasPrevious);
        Index -= (int)count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public SyntaxToken? Peek(int offset = 0) => Index + offset < Tokens.Count ? Tokens[Index + offset] : null;

    public SyntaxToken? ConsumeOptional(SyntaxKind kind)
    {
        if (Current.Kind.HasFlag(kind))
        {
            var aux = Current;
            Advance();
            return aux;
        }

        return null;
    }

    public string DisplayAtSource()
    {
        if (!HasCurrent)
            return string.Empty;

        var builder = new StringBuilder();
        int insertIndex = Current.Position.Index;

        // Find the next newline character after the insert index
        var newLineIndex = Source.IndexOf('\n', insertIndex);
        if (newLineIndex == -1)
        {
            // If there's no newline character, append the rest of the source
            builder.Append(Source);
        }
        else
        {
            // Extract the leading whitespace from the line where we are inserting
            string leadingWhitespace = new string(Source.Substring(newLineIndex + 1)
                .TakeWhile(char.IsWhiteSpace)
                .ToArray());

            // Append the substring before the insert index
            builder.Append(Source.AsSpan(0, newLineIndex));
            builder.AppendLine();

            // Append the leading whitespace and the inserted line
            builder.Append(leadingWhitespace);
            builder.Append("^-----------");
            builder.Append('\n');

            // Append the substring after the insert index
            builder.Append(Source.AsSpan(newLineIndex));
        }

        return builder.ToString();
    }

    #endregion
}