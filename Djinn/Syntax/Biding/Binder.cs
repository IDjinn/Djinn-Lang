using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;

namespace Djinn.Syntax.Biding;

public class Binder : IStatementVisitor<IBoundStatement>, IExpressionVisitor<IBoundExpression>
{
    private Reporter _reporter;

    public Binder()
    {
        _reporter = new Reporter();
    }

    public IBoundExpression VisitBinaryExpression(BinaryExpressionSyntax expressionSyntax, Scope scope)
    {
        return BindBinaryExpression(expressionSyntax,scope);
    }

    public IBoundExpression VisitConstantNumberExpression(ConstantNumberExpressionSyntax expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitConstantStringExpression(ConstantStringExpressionSyntax expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitUnaryExpression(UnaryExpressionSyntax expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitParameterDeclaration(ParameterDeclaration declarationSyntax, Scope scope)
    {
        return BindExpression(declarationSyntax, scope);
    }

    public IBoundExpression VisitNoOpExpression(NoOpExpression expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitIdentifierExpression(IdentifierExpression expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitFunctionCallExpression(FunctionCallExpression expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitConstantBooleanExpression(ConstantBooleanExpression expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitAssigmentExpression(AssigmentExpression expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundExpression VisitArgumentsExpression(ArgumentsExpression expressionSyntax, Scope scope)
    {
        return BindExpression(expressionSyntax, scope);
    }

    public IBoundStatement Visit(DiscardExpressionResultStatement discardExpressionResult, Scope scope)
    {
        return BindStatement(discardExpressionResult, scope);
    }

    public IBoundStatement Visit(FunctionStatement functionStatement, Scope scope)
    {
        throw new NotImplementedException();
    }

    public IBoundStatement Visit(ReturnStatement returnStatement, Scope scope)
    {
        return BindReturnStatement(returnStatement,scope);
    }

    public IBoundStatement Visit(BlockStatement blockStatement, Scope scope)
    {
        return BindBlockStatement(blockStatement,scope);
    }

    public IBoundStatement Visit(FunctionDeclarationStatement functionDeclarationStatement, Scope scope)
    {
        return BindFunctionStatement(functionDeclarationStatement,scope);
    }

    public IEnumerable<IBoundStatement> Bind(SyntaxTree syntaxTree)
    {
        var globalScope = new Scope("global");
        var statements = new List<IBoundStatement>();
        foreach (var statement in syntaxTree.Statements)
        {
            statements.Add(BindStatement(statement, globalScope));
        }

        return statements;
    }

    public IBoundStatement BindStatement(IStatement statement, Scope scope)
    {
        return statement switch
        {
            ReturnStatement returnStatement => BindReturnStatement(returnStatement, scope),
            BlockStatement blockStatement => BindBlockStatement(blockStatement,scope),
            FunctionDeclarationStatement functionDeclaration => BindFunctionStatement(functionDeclaration, scope),
            _ => _reporter.Error($"Unsupported binding statement of type '{statement.GetType().Name}'",
                BoundBlockStatement.Empty)
        };
    }


    public IEnumerable<IBoundStatement> BoundStatements(IEnumerable<IStatement> statements, Scope scope)
    {
        // yes, we need make it foreach intentionally array because IDE doesn't support yielded return statements while debugging
        var stats = statements.ToArray();
        var bounds = new List<IBoundStatement>(stats.Length);
        foreach (var statement in stats)
        {
            bounds.Add(BindStatement(statement,scope));
        }

        return bounds;
    }

    private BoundBlockStatement BindBlockStatement(BlockStatement blockStatement, Scope scope)
    {
        var blockScope = new Scope("block-scope", scope);
        return new BoundBlockStatement(BoundStatements(blockStatement.Statements,blockScope));
    }

    private BoundReturnStatement BindReturnStatement(ReturnStatement returnStatement, Scope scope)
    {
        return new BoundReturnStatement(BindExpression(returnStatement.ExpressionSyntax, scope));
    }

    private BoundFunctionStatement BindFunctionStatement(FunctionDeclarationStatement functionDeclarationStatement, Scope scope)
    {
        var functionNameIdentifier = new BoundIdentifier((string)functionDeclarationStatement.Identifier.Value);
        var functionScope = new FunctionScope(functionNameIdentifier.Name, scope);
        var paramsDeclaration = BindFunctionParameters(functionDeclarationStatement, functionScope);
        var body = BindStatement(functionDeclarationStatement.Statement,functionScope);
        return new BoundFunctionStatement(
            functionNameIdentifier,
            paramsDeclaration,
            body
        );
    }

    private IEnumerable<BoundParameter> BindFunctionParameters(
        FunctionDeclarationStatement functionDeclarationStatement, FunctionScope scope)
    {
        var parameters = new List<BoundParameter>(functionDeclarationStatement.Parameters.Count());
        foreach (var parameterDeclaration in functionDeclarationStatement.Parameters)
        {
            var type = new BoundIdentifier((string)parameterDeclaration.Type.Value, BoundNodeKind.FunctionParameter);
            var identifier = new BoundIdentifier((string)parameterDeclaration.Identifier.Value, BoundNodeKind.FunctionParameter);
            BoundLiteralExpression? defaultValue = null;
            if (parameterDeclaration.DefaultValue is not null)
            {
                defaultValue = BindExpression(parameterDeclaration.DefaultValue, scope) as BoundLiteralExpression;
            }

            var parameter = new BoundParameter(
                type,
                identifier,
                defaultValue
            );
            
            parameters.Add(parameter);
            scope.TryAddParameter(parameter);
        }

        return parameters;
    }

    public BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax, Scope scope)
    {
        var boundBinaryOperator =
            BoundBinaryOperator.Bind(binaryExpressionSyntax.Operator.Kind, new Integer32(), new Integer32());

        return new BoundBinaryExpression
        {
            Left = BindExpression(binaryExpressionSyntax.LeftExpression, scope),
            Operator = boundBinaryOperator,
            Right = BindExpression(binaryExpressionSyntax.RightExpression, scope)
        };
    }

    public IBoundExpression BindExpression(IExpressionSyntax expressionSyntax, Scope scope)
    {
        return expressionSyntax switch
        {
            BinaryExpressionSyntax binary => BindBinaryExpression(binary, scope),
            UnaryExpressionSyntax unary => BindUnaryExpression(unary, scope),
            ConstantNumberExpressionSyntax constantNumber => BindLiteralNumber(constantNumber, scope),
            ConstantStringExpressionSyntax constantString => BindLiteralString(constantString, scope),
            ConstantBooleanExpression constantBoolean => BindLiteralBoolean(constantBoolean, scope),
            FunctionCallExpression functionCallExpression => BindFunctionCallExpression(functionCallExpression, scope),
            IdentifierExpression nameExpression => BindIdentifierExpression(nameExpression, scope),
            _ => throw new NotImplementedException()
        };
    }

    private IBoundExpression BindIdentifierExpression(IdentifierExpression identifierExpression, Scope scope)
    {
        var identifier = (string) identifierExpression.Identifier.Value;
        var variable = scope.FindVariable(identifier);


        return default;

    }

    private IBoundExpression BindFunctionCallExpression(FunctionCallExpression functionCallExpression, Scope scope)
    {
        return default;
    }

    private BoundLiteralExpression BindLiteralBoolean(ConstantBooleanExpression constantBoolean, Scope scope)
    {
        var boolean = constantBoolean.Bool.Value.ToString()!.Equals("true");
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Type = new Bool(boolean),
                Value = new Bool(boolean)
            }
        };
    }

    private BoundLiteralExpression BindLiteralString(ConstantStringExpressionSyntax constantString, Scope scope)
    {
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Value = new String(constantString.StringToken.Value.ToString()!),
                Type = new String(constantString.StringToken.Value.ToString()!)
            }
        };
    }

    private BoundLiteralExpression BindLiteralNumber(ConstantNumberExpressionSyntax constantNumber, Scope scope)
    {
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Value = new Integer32((int)constantNumber.NumberToken.Value),
                Type = new Integer32((int)constantNumber.NumberToken.Value)
            }
        };
    }

    private BoundUnaryExpression BindUnaryExpression(UnaryExpressionSyntax unary, Scope scope)
    {
        var boundOperator = BoundUnaryOperator.Bind(unary.Operator.Kind, new Integer32());
        if (boundOperator is null)
        {
            // TODO REPORTING
        }

        var operand = BindExpression(unary.Operand, scope);
        if (unary.Operand is not ConstantNumberExpressionSyntax cnst)
        {
            throw new NotImplementedException(); // todo: unary must be compile-time handled. (sum, minus, div, mod) and other types
        }
        
        return new BoundUnaryExpression()
        {
            OperandExpression = operand,
            Operator = boundOperator,
        };
    }
}