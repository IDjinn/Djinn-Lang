using System.Diagnostics.CodeAnalysis;
using Djinn.Expressions;
using Djinn.Lexing;
using Djinn.Statements;
using Djinn.Syntax;
using Djinn.Utils;

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
        return Current.Kind switch
        {
            SyntaxKind.VariableDeclaration => ParseVariableDeclaration(),
            SyntaxKind.VariableAssignmentExpression => ParseVariableAssignmentExpression(),
            SyntaxKind.FunctionDeclaration => ParseFunctionDeclaration(),
            SyntaxKind.OpenBrace => ParseBlockStatement(),
            SyntaxKind.ReturnDeclaration => ParseReturnStatement(),
            SyntaxKind.Identifier => ParseIdentifier(),
            _ => ExpectingStatement(),
        };
    }

    private IStatement ParseIdentifier()
    {
        var expression = ParsePrimaryExpression();
        return expression switch
        {
            FunctionCallExpression call => new DiscardExpressionResultStatement(call)
        };
    }

    private IStatement ExpectingStatement()
    {
        return DiagnosticError<BlockStatement>("Expecting statement.");
    }

    private IStatement ParseReturnStatement()
    {
        Consume(SyntaxKind.ReturnDeclaration);
        var expression = ParseExpression();
        return new ReturnStatement(expression);
    }

    private IStatement ParseBlockStatement()
    {
        Consume(SyntaxKind.OpenBrace);
        var statements = new List<IStatement>();
        while (HasCurrent && !TryMatchExact(SyntaxKind.CloseBrace, out _))
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
        var parameters = ParseParametersStatement();
        var statement = ParseStatement();

        return new FunctionDeclarationStatement(
            type, identifier, parameters, statement
        );
    }

    private ParametersDeclarationStatement ParseParametersStatement()
    {
        Consume(SyntaxKind.OpenParenthesis);
        var parameters = new List<ParameterExpression>();
        while (Match(SyntaxKind.Type))
            parameters.Add(ParseParameterExpression());
        Consume(SyntaxKind.CloseParenthesis);

        return new ParametersDeclarationStatement(parameters);
    }

    private ParameterExpression ParseParameterExpression()
    {
        var type = Consume(SyntaxKind.Type);
        if (type.Kind.HasFlag(SyntaxKind.Void))
            return new ParameterExpression(ParameterExpression.VoidIdentifier, ParameterExpression.VoidIdentifier);

        if (!TryMatch(SyntaxKind.Identifier, out var identifier))
        {
            DiagnosticError<bool>("Invalid parameter declaration");
            return ParameterExpression.BadParameters;
        }

        var paramIdentifier = identifier with { Kind = SyntaxKind.ParamVariableIdentifier };
        var equals = ConsumeOptional(SyntaxKind.EqualsOperator);
        var defaultValue = ConsumeOptional(SyntaxKind.Constant);
        if (equals is not null && defaultValue is null)
        {
            DiagnosticError<bool>("Invalid parameter declaration");
            return ParameterExpression.BadParameters;
        }

        return new ParameterExpression(paramIdentifier, type);
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
        Consume(SyntaxKind.VariableDeclaration);
        var identifier = Consume(SyntaxKind.Identifier);
        var operatorToken = Consume(SyntaxKind.LogicalOperators);
        var expression = ParsePrimaryExpression();
        return new AssigmentExpression(identifier, operatorToken, expression);
    }


    public IExpressionSyntax ParseExpression(int parentPrecedence = 0)
    {
        IExpressionSyntax left;
        var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
        if (unaryOperatorPrecedence == SyntaxKindExtensions.InvalidOperatorPrecedence ||
            parentPrecedence > unaryOperatorPrecedence)
        {
            left = ParsePrimaryExpression();
        }
        else
        {
            var operatorToken = Consume(SyntaxKind.ArithmeticOperators);
            var operand = ParseExpression(unaryOperatorPrecedence);
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
            if (TryMatch(SyntaxKind.OpenParenthesis, out var openParenthesisToken))
            {
                var fn = new FunctionCallExpression(identifierToken, ParseArgumentsExpression());
                Consume(SyntaxKind.CloseParenthesis);
                return fn;
            }

            return new NameExpression(identifierToken);
        }

        return DiagnosticError<NoOpExpression>("Invalid expression syntax");
    }

    private ArgumentsExpression ParseArgumentsExpression()
    {
        var args = new List<IExpressionSyntax>();
        while (!TryMatch(SyntaxKind.CloseParenthesis, out var _))
            args.Add(ParseArgument());

        return new ArgumentsExpression(args);
    }

    private IExpressionSyntax ParseArgument()
    {
        return ParsePrimaryExpression();
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
        _diagnostics.Add(new Diagnostic(new Position(_index), message, severity));
        _index++;
        return default!;
    }
}