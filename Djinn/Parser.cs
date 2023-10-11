using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Djinn.Expressions;
using Djinn.Statements;
using Djinn.SyntaxNodes;
using Djinn.Utils;

namespace Djinn;

public class Parser
{
    private static readonly SyntaxKind[] IgnoredKinds = new[]
    {
        SyntaxKind.WhiteSpaceToken,
        SyntaxKind.BadToken
    };
    
    private readonly IList<SyntaxToken> _tokens = new List<SyntaxToken>();
    private readonly IList<Diagnostic> _diagnostics = new List<Diagnostic>();
    private readonly Lexer _lexer;
    private int _index;

    public Parser(Lexer lexer)
    {
        _lexer = lexer;
        while (!lexer.EOF)
        {
            var token = _lexer.NextToken();
            if(IgnoredKinds.Contains(token.Kind))
                continue;
            
            _tokens.Add(token);
        }

    }

    public SyntaxTree Parse()
    {
        var nodes= new List<IStatement>() { ParseStatement() };
        return new SyntaxTree
        {
            Diagnostics = _diagnostics.AsReadOnly(),
            Statements = nodes
        };
    }

    public IStatement? ParseStatement()
    {
        return Current?.Kind switch
        {
            SyntaxKind.VariableDeclaration => ParseVariableDeclaration(),
            SyntaxKind.VariableAssignment => ParseVariableAssignment(),
            SyntaxKind.FunctionDeclaration => ParseFunctionDeclaration(),
            SyntaxKind.OpenBrace => ParseBlockStatement(),
            SyntaxKind.ReturnDeclaration => ParseReturnStatement(),
            _ => null,
        };
    }

    private IStatement ParseReturnStatement()
    {
        Consume(SyntaxKind.ReturnDeclaration);
        var expression = ParseExpression();
        var returnType = SyntaxToken.BadToken;
        return new ReturnStatement(returnType, expression!);
    }

    private IStatement ParseBlockStatement()
    {
        Consume(SyntaxKind.OpenBrace);
        var statements = new List<IStatement>();
        while (HasCurrent && !TryMatch(SyntaxKind.CloseBrace, out _))
            statements.Add(ParseStatement()!);
        Consume(SyntaxKind.CloseBrace);

        return new BlockStatement(statements);
    }

    private IStatement? ParseFunctionDeclaration()
    {
        var function = Consume(SyntaxKind.FunctionDeclaration);
        var type = Consume(SyntaxKind.Void);
        var identifier = Consume(SyntaxKind.Identifier);       
        ParseParametersStatement();
        var statement = ParseStatement();
        //if(!statement.Type.Kind.IsValueType() || statement.Type.Kind == SyntaxKind.Void)
        //    return null;
        
        return statement;
    }

    private void ParseParametersStatement()
    {
        Consume(SyntaxKind.OpenParenthesis);
        ConsumeOptional(SyntaxKind.Void);
        Consume(SyntaxKind.CloseParenthesis);
    }

    private IStatement? ParseVariableAssignment()
    {
        throw new NotImplementedException();
    }

    private IStatement? ParseVariableDeclaration()
    {
        throw new NotImplementedException();
    }

    
    public IExpressionSyntax? ParseExpression()
    {
        var leftExpression = ParsePrimaryExpression();
        while (leftExpression is not null && HasCurrent && Current.Kind.IsLogicOperator())
        {
            var operatorToken = Advance()!; // TODO:DIAGNOSTICS
            var rightExpression = ParsePrimaryExpression();
            if(rightExpression is null)
            {
                DiagnosticError<bool>("Expected expression after operator");
                break;
            }
            
            leftExpression = new BinaryExpressionSyntax(leftExpression, operatorToken, rightExpression);
        }

        return leftExpression;
    }

    public IExpressionSyntax? ParsePrimaryExpression()
    {
        if (TryMatch(SyntaxKind.StringLiteral, out var stringToken))
            return new ConstantStringExpressionSyntax(stringToken);
        if(TryMatch(SyntaxKind.NumberLiteral, out var numberToken))
            return new ConstantNumberExpressionSyntax(numberToken);
        if (TryMatch(SyntaxKind.Identifier, out var identifierToken))
        {
            if (TryMatch(SyntaxKind.OpenParenthesis, out var openParenthesisToken))
            {
                var fn = new FunctionCallExpression(ParseExpression()!);
                Consume(SyntaxKind.CloseParenthesis);
                return fn;
            }
        }
        
        return DiagnosticError<IExpressionSyntax>("Invalid expression syntax");
    }

    public bool Match(SyntaxKind kind)
    {
        return HasCurrent && Current.Kind == kind;
    }

    public bool TryMatch(SyntaxKind kind, [NotNullWhen(true)] out SyntaxToken? token)
    {
        return TryMatch<SyntaxToken>(kind, out token);
    }
    public bool TryMatch<TCast>(SyntaxKind kind, [NotNullWhen(true)] out TCast? token) where TCast : SyntaxToken
    {
        token = default;
        if(!HasCurrent)
            return DiagnosticError<bool>($"Expected token kind '{kind}' but it's out of bounds");

        if (Current.Kind != kind)
            return false;

        var current = Current;
        token = (TCast)current;
        Advance();
        return true;
    }

    public SyntaxToken? Peek(int offset = 0)
    {
        if (!HasCurrent)
        {
            DiagnosticError<SyntaxToken>($"Out of bounds for current token");
            return SyntaxToken.BadToken;
        }
        
        var skip = _index + offset;
        if (skip >= _tokens.Count)
        {
            DiagnosticError<SyntaxToken>($"Out of bounds to peek offset {offset} token");
            return SyntaxToken.BadToken;
        }

        return _tokens[skip];
    }

    public bool HasNext => _index + 1 < _tokens.Count;
    [MemberNotNullWhen(true, nameof(Current))]
    public bool HasCurrent => _index < _tokens.Count;
    public SyntaxToken? Current => Peek();

    public SyntaxToken? ConsumeOptional(SyntaxKind kind, int offset = 0)
    {
        var token = Peek(offset);
        if (token?.Kind != kind)
        {
            return null;
        }
        
        _index += offset + 1;
        return token;
    }
    public SyntaxToken Consume(SyntaxKind kind, int offset = 0)
    {
        var token = Peek(offset);
        if (token?.Kind != kind)
        {
            DiagnosticError<SyntaxToken>($"Expected token of kind '{kind}' but got '{token?.Kind}'");
            return SyntaxToken.BadToken;
        }
        
        _index += offset + 1;
        return token;
    }
    public SyntaxToken? Advance()
    {
        if (!HasCurrent)
        {
            DiagnosticError<SyntaxToken>($"Out of bounds for current token");
            return SyntaxToken.BadToken;
        }
        
        var current = Current;
        _index++;
        return current;
    }

    public T? DiagnosticError<T>( string message)
    {
        return Diagnostic<T>(DiagnosticSeverity.Error, message);
    }
    public T? DiagnosticWarn<T>( string message)
    {
        return Diagnostic<T>(DiagnosticSeverity.Warning, message);
    }
    
    public T? Diagnostic<T>(DiagnosticSeverity severity, string message)
    {
        _diagnostics.Add(new Diagnostic(new Position(_index),message, severity));
        // if(HasCurrent)
        //     Advance();
        _index++;
        return default;
    }
}