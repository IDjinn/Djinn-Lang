using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Djinn.Expressions;
using Djinn.Lexing;
using Djinn.Statements;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BinaryExpressionSyntax = Djinn.Expressions.BinaryExpressionSyntax;

namespace Djinn.Parsing;

public class Parser
{
    private static readonly SyntaxKind[] IgnoredKinds = new[]
    {
        SyntaxKind.WhiteSpaceToken,
        SyntaxKind.BadToken
    };

    private readonly IList<Diagnostic> _diagnostics = new List<Diagnostic>();
    private readonly Lexer _lexer;

    private readonly IList<SyntaxToken> _tokens = new List<SyntaxToken>();
    private int _index;

    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics.AsReadOnly();

    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        while (!lexer.EOF)
        {
            var token = _lexer.NextToken();
            if (IgnoredKinds.Contains(token.Kind))
                continue;

            _tokens.Add(token);
        }
    }

    private bool IsEOF => _index >= _tokens.Count;

    public bool HasNext => _index + 1 < _tokens.Count;

    [MemberNotNullWhen(true, nameof(Current))]
    public bool HasCurrent => _index < _tokens.Count;

    public SyntaxToken Current => Peek();

    public SyntaxTree Parse()
    {
        var nodes = new List<IStatement> { };
        while (!IsEOF)
            nodes.Add(ParseStatement());

        return new SyntaxTree
        {
            Diagnostics = _diagnostics.AsReadOnly(),
            Statements = nodes
        };
    }

    public IStatement ParseStatement()
    {
        if (CurrentIsType() && TryParseVariableDeclaration(out var statement)) return statement;
        
        return Current.Kind switch
        {
            SyntaxKind.FunctionDeclaration => ParseFunctionDeclaration(), // TODO: IS THIS USED?
            SyntaxKind.OpenBrace => ParseBlockStatement(),
            SyntaxKind.ReturnDeclaration => ParseReturnStatement(),
            SyntaxKind.Identifier => ParseIdentifier(),
            SyntaxKind.Import => ParseImport(),
            SyntaxKind.Switch => ParseSwitchStatement(),
            
            SyntaxKind.BadToken or SyntaxKind.EndOfFileToken => throw new ArgumentException("EOF"),
            _ => ExpectingStatement(),
        };
    }

    private bool TryParseVariableDeclaration([NotNullWhen(true)] out VariableDeclarationStatement? statement)
    {
        statement = default;
        var type = Consume(SyntaxKind.Type);
        if (!TryMatch(SyntaxKind.Identifier, out var identifier))
            return false;

        if (!TryMatch(SyntaxKind.EqualsOperator, out var _))
            return false;

        var expression = ParsePrimaryExpression();
        statement = new VariableDeclarationStatement(
            type,
            identifier,
            expression
            );
        return true;
    }

    private bool CurrentIsType()
    {
        return Current.Kind.HasFlag(SyntaxKind.Type);
    }

    private SwitchStatement ParseSwitchStatement()
    {
        Consume(SyntaxKind.Switch);
        Consume(SyntaxKind.OpenParenthesis);
        var switching = ParsePrimaryExpression();
        Consume(SyntaxKind.CloseParenthesis);
        Consume(SyntaxKind.OpenBrace);
        var cases = new List<SwitchCaseStatement>();
        while (!TryMatch(SyntaxKind.CloseBrace, out _))
        {
            var caseOrDefault = ConsumeOptional(SyntaxKind.Case) ?? ConsumeOptional(SyntaxKind.Default);
            if (caseOrDefault is null)
            {
                DiagnosticError<bool>("Expecting case or default statement of switch expression.");
                continue;
            }

            if (caseOrDefault.Kind == SyntaxKind.Default)
            {
                cases.Add(new SwitchCaseStatement(null, ParseBlockStatement()));
                continue;
            }
            
            cases.Add(new SwitchCaseStatement(ParsePrimaryExpression(), ParseBlockStatement()));
        }
        return new SwitchStatement(switching, cases);
    }

    private IStatement ParseImport()
    {
        var importKeyword = Consume(SyntaxKind.Import);
        var targetLibrary = Consume(SyntaxKind.StringLiteral);
        return new ImportStatement(importKeyword, targetLibrary);
    }

    private IStatement ParseIdentifier()
    {
        var keyword = KeywordExtensions.FromString((string)Current.Value);
        switch (keyword)
        {  
            case Keyword.If:
                return ParseIfStatement();
            case Keyword.Else:
                return ParseElseStatement();
        }
        
        
        var expression = ParsePrimaryExpression();
        return expression switch
        {
            FunctionCallExpression call => new DiscardExpressionResultStatement(call),
            _ => throw new NotImplementedException(expression.GetType().Name)
        };
    }

    private IStatement ExpectingStatement()
    {
        var aux = DiagnosticError<BlockStatement>("Expecting statement.");
        Debug.Fail("Expecting statement.");
        return aux;
    }

    private IStatement ParseReturnStatement()
    {
        Consume(SyntaxKind.ReturnDeclaration);
        var expression = ParseExpression();
        return new ReturnStatement(expression);
    }

    private BlockStatement ParseBlockStatement()
    {
        Consume(SyntaxKind.OpenBrace);
        var statements = new List<IStatement>();
        while (HasCurrent && !TryMatchExact(SyntaxKind.CloseBrace, out var test))
        {
            statements.Add(ParseStatement()!);
        }

        return new BlockStatement(statements);
    }

    private FunctionDeclarationStatement ParseFunctionDeclaration()
    {
        Consume(SyntaxKind.FunctionDeclaration);
        var type = Consume(SyntaxKind.Type); // TODO what i will do with tat 
        var identifier = Consume(SyntaxKind.Identifier);
        var parameters = ParseFunctionParametersDeclaration();
        var statement = ParseStatement();

        return new FunctionDeclarationStatement(
            type, identifier, parameters, statement
        );
    }

    private IEnumerable<ParameterDeclaration> ParseFunctionParametersDeclaration()
    {
        Consume(SyntaxKind.OpenParenthesis);
        var parameters = new List<ParameterDeclaration>();
        while (Match(SyntaxKind.Type))
        {
            parameters.Add(ParseParameterExpression());
        }
        Consume(SyntaxKind.CloseParenthesis);

        return parameters;
    }

    private ParameterDeclaration ParseParameterExpression()
    {
        var type = Consume(SyntaxKind.Type);
        if (type.Kind.HasFlag(SyntaxKind.Void))
            return ParameterDeclaration.VoidParameter;

        if (!TryMatch(SyntaxKind.Identifier, out var identifier))
        {
            DiagnosticError<bool>("Invalid parameter declaration");
            return ParameterDeclaration.BadParameters;
        }

        var paramIdentifier = identifier with { Kind = SyntaxKind.ParamVariableIdentifier };
        var equals = ConsumeOptional(SyntaxKind.EqualsOperator);
        var defaultValue = ConsumeOptional(SyntaxKind.Constant);
        if (equals is not null && defaultValue is null)
        {
            DiagnosticError<bool>("Invalid parameter declaration");
            return ParameterDeclaration.BadParameters;
        }

        return new ParameterDeclaration(paramIdentifier, type, null); // TODO DEFAULT VALUE EXPRESSION PARSING & BINDING
    }


    private IStatement ParseVariableAssignmentExpression()
    {
        throw new NotImplementedException();
    }

    private IStatement ParseVariableDeclaration()
    {
        return new ReturnStatement(ParseAssignmentExpression()); // TODO 
    }


    public IExpressionSyntax ParseAssignmentExpression()
    {
        // TODO Consume(SyntaxKind.VariableDeclaration);
        var identifier = Consume(SyntaxKind.Identifier);
        var operatorToken = Consume(SyntaxKind.LogicalOperators);
        var expression = ParseExpression();
        return new AssigmentExpression(identifier, operatorToken, expression);
    }


    public IExpressionSyntax ParseExpression(int parentPrecedence = 0)
    {
        IExpressionSyntax left;        
        var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
        if (!unaryOperatorPrecedence.HasValue  || parentPrecedence > unaryOperatorPrecedence.Value)
        {
            left = ParsePrimaryExpression();
        }
        else
        {
            var operatorToken = Consume(SyntaxKind.ArithmeticOperators);
            var operand = ParseExpression(unaryOperatorPrecedence.Value);
            left = new UnaryExpressionSyntax(operand, operatorToken);
        }

        while (true)
        {
            var precedence = Current.Kind.GetBinaryOperatorPrecedence();
            if (precedence == SyntaxKindExtensions.InvalidOperatorPrecedence || precedence <= parentPrecedence)
                break;

            var operatorToken = Consume(SyntaxKind.LogicalOperators);
            var right = ParseExpression(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken, right);
        }

        return left;
    }
    public IExpressionSyntax ParsePrimaryExpression()
    {
        if (TryMatch(SyntaxKind.EndOfFileToken, out var eof))
            throw new Exception("eof");
        if (TryMatch(SyntaxKind.BadToken, out var badToken))
            throw new Exception("badtoken");
        
        if (TryMatch(SyntaxKind.StringLiteral, out var stringToken))
            return new ConstantStringExpressionSyntax(stringToken);
        if (TryMatch(SyntaxKind.NumberLiteral, out var numberToken))
            return new ConstantNumberExpressionSyntax(numberToken);
        if (TryMatch(SyntaxKind.True, out var trueToken))
            return new ConstantBooleanExpression(trueToken);
        if (TryMatch(SyntaxKind.True, out var falseToken))
            return new ConstantBooleanExpression(falseToken);

        if (TryMatch(SyntaxKind.Identifier, out var identifierToken))
        {
            switch (KeywordExtensions.FromString((string)identifierToken.Value))
            {
                // case Keyword.If:
                //     return ParseIfStatement();
                // case Keyword.Else:
                //     return ParseElseStatement();

                case Keyword.Try:
                case Keyword.Catch:
                case Keyword.Finally:
                    throw new NotImplementedException();

                default:
                    if (TryMatch(SyntaxKind.OpenParenthesis, out var _))
                        return new FunctionCallExpression(identifierToken, ParseArgumentsExpression());

                    return new IdentifierExpression(identifierToken);
            }
        }

        return DiagnosticError<NoOpExpression>("Invalid expression syntax");
    }

    private IStatement ParseElseStatement()
    {
        throw new NotImplementedException();
    }

    private IfStatement ParseIfStatement()
    {
        Consume(SyntaxKind.Identifier);
        Consume(SyntaxKind.OpenParenthesis);
        var conditional = ParseBooleanExpression();
        Consume(SyntaxKind.CloseParenthesis);
        var body = ParseBlockStatement();

        return new IfStatement(
            conditional,
            body
        );
    }

    private IExpressionSyntax ParseBooleanExpression()
    {
        var expression = ParsePrimaryExpression();
        if (expression is not null)
        {
            return expression;
        }
        return DiagnosticError<ConstantBooleanExpression>("Invalid expression syntax");
    }

    private IEnumerable<IExpressionSyntax> ParseArgumentsExpression()
    {
        var args = new List<IExpressionSyntax>();
        while (!TryMatch(SyntaxKind.CloseParenthesis, out var _))
        {
            var exp = ParsePrimaryExpression() ?? throw new Exception();
            args.Add(exp);
        }

        return args;
    }


    public bool Match(SyntaxKind kind)
    {
        return HasCurrent && (Current.Kind.HasFlag(kind) || Current.Kind == kind);
    }

    public bool MatchExact(SyntaxKind kind)
    {
        return HasCurrent && Current.Kind == kind;
    }

    public bool TryMatch(SyntaxKind kind, [NotNullWhen(true)] out SyntaxToken? token)
    {
        return TryMatch<SyntaxToken>(kind, out token);
    }

    public bool TryMatchExact(SyntaxKind kind, [NotNullWhen(true)] out SyntaxToken? token)
    {
        return TryMatchExact<SyntaxToken>(kind, out token);
    }

    public bool TryMatchExact<TCast>(SyntaxKind kind, [NotNullWhen(true)] out TCast? token) where TCast : SyntaxToken
    {
        token = default;
        if (!MatchExact(kind))
            return false;

        var current = Current;
        token = (TCast)current;
        Advance();
        return true;
    }

    public bool TryMatch<TCast>(SyntaxKind kind, [NotNullWhen(true)] out TCast? token) where TCast : SyntaxToken
    {
        token = default;
        if (!Match(kind))
            return false;

        var current = Current;
        token = (TCast)current;
        Advance();
        return true;
    }

    public SyntaxToken Peek(int offset = 0)
    {
        var skip = _index + offset;
        if (skip >= _tokens.Count)
        {
            Debug.Fail($"Out of bounds to peek offset {offset} token");
            DiagnosticError<SyntaxToken>($"Out of bounds to peek offset {offset} token");
            return SyntaxToken.BadToken;
        }

        return _tokens[skip];
    }

    public SyntaxToken? ConsumeOptional(SyntaxKind kind, int offset = 0)
    {
        var token = Peek(offset);
        if (!token.Kind.HasFlag(kind) && token.Kind != kind)
            return null;

        _index += offset + 1;
        return token;
    }

    public SyntaxToken Consume(SyntaxKind kind, int offset = 0)
    {
        var token = Peek(offset);
        if (!token.Kind.HasFlag(kind) && token.Kind != kind)
        {
            Debug.Fail($"Expected token of kind '{kind}' but got '{token?.Kind}'");
            DiagnosticError<SyntaxToken>($"Expected token of kind '{kind}' but got '{token?.Kind}'");
            return SyntaxToken.BadToken;
        }

        _index += offset + 1;
        return token;
    }

    public SyntaxToken Advance()
    {
        var current = Current;
        _index++;
        return current;
    }

    public T DiagnosticError<T>(string message)
    {
        return Diagnostic<T>(DiagnosticSeverity.Error, message);
    }

    public T? DiagnosticWarn<T>(string message)
    {
        return Diagnostic<T>(DiagnosticSeverity.Warning, message);
    }

    public T Diagnostic<T>(DiagnosticSeverity severity, string message)
    {
        Debug.Fail(message);
        _diagnostics.Add(new Diagnostic(new Position(_index), message, severity));
        _index++;
        return default!;
    }
}