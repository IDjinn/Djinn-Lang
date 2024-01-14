using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax.Biding.Expressions;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Syntax.Biding.Scopes.Variables;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;

namespace Djinn.Syntax.Biding;

public class Binder : IStatementVisitor<IBoundStatement>, IExpressionVisitor<IBoundExpression>
{
    public readonly Reporter Reporter = new ();


    public IBoundExpression VisitBinaryExpression(BinaryExpressionSyntax expressionSyntax, BoundScope boundScope)
    {
        return BindBinaryExpression(expressionSyntax,boundScope);
    }

    public IBoundExpression VisitConstantNumberExpression(ConstantNumberExpressionSyntax expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitConstantStringExpression(ConstantStringExpressionSyntax expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitUnaryExpression(UnaryExpressionSyntax expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitParameterDeclaration(ParameterDeclaration declarationSyntax, BoundScope boundScope)
    {
        return BindExpression(declarationSyntax, boundScope);
    }

    public IBoundExpression VisitNoOpExpression(NoOpExpression expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitIdentifierExpression(IdentifierExpression expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitFunctionCallExpression(FunctionCallExpression expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitConstantBooleanExpression(ConstantBooleanExpression expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitAssigmentExpression(AssigmentExpression expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundExpression VisitArgumentsExpression(ArgumentsExpression expressionSyntax, BoundScope boundScope)
    {
        return BindExpression(expressionSyntax, boundScope);
    }

    public IBoundStatement Visit(DiscardExpressionResultStatement discardExpressionResult, BoundScope boundScope)
    {
        return BindStatement(discardExpressionResult, boundScope);
    }

    public IBoundStatement Visit(FunctionStatement functionStatement, BoundScope boundScope)
    {
        throw new NotImplementedException();
    }

    public IBoundStatement Visit(ReturnStatement returnStatement, BoundScope boundScope)
    {
        return BindReturnStatement(returnStatement,boundScope);
    }

    public IBoundStatement Visit(BlockStatement blockStatement, BoundScope boundScope)
    {
        return BindBlockStatement(blockStatement,boundScope);
    }

    public IBoundStatement Visit(FunctionDeclarationStatement functionDeclarationStatement, BoundScope boundScope)
    {
        return BindFunctionStatement(functionDeclarationStatement,boundScope);
    }

    public IBoundStatement Visit(IfStatement functionDeclarationStatement, BoundScope boundScope)
    {
        throw new NotImplementedException();
    }

    public IBoundStatement Visit(ImportStatement importStatement, BoundScope boundScope)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IBoundStatement> Bind(SyntaxTree syntaxTree)
    {
        var globalScope = new BoundGlobalScope("global");
        globalScope.Init();
        var statements = new List<IBoundStatement>();
        foreach (var statement in syntaxTree.Statements)
        {
            statements.Add(BindStatement(statement, globalScope));
        }

        return statements;
    }

    public IBoundStatement BindStatement(IStatement statement, BoundScope boundScope)
    {
        return statement switch
        {
            ReturnStatement returnStatement => BindReturnStatement(returnStatement, boundScope),
            BlockStatement blockStatement => BindBlockStatement(blockStatement,boundScope),
            FunctionDeclarationStatement functionDeclaration => BindFunctionStatement(functionDeclaration, boundScope),
            DiscardExpressionResultStatement discartExpressionResult => BindDiscartExpressionResult(discartExpressionResult, boundScope),
            IfStatement ifStatement => BindIfStatement(ifStatement, boundScope),
           ImportStatement importStatement => BindImportStatement(importStatement,boundScope),
            _ => Reporter.Error($"Unsupported binding statement of type '{statement.GetType().Name}'",
                BoundBlockStatement.Empty)
        };
    }

    private IBoundStatement BindImportStatement(ImportStatement importStatement, BoundScope boundScope)
    {
        // TODO: RESOLVE LIBRARY BY SCOPE
        return new BoundImportStatement((string)importStatement.Library.Value);
    }

    private IBoundStatement BindIfStatement(IfStatement ifStatement, BoundScope boundScope)
    {
        var ifScope = new BoundScope("if", boundScope);
        return new BoundIfStatement(BindExpression(ifStatement.Conditional, ifScope),
            BindStatement(ifStatement.Block, ifScope));
    }

    private IBoundStatement BindDiscartExpressionResult(DiscardExpressionResultStatement discartExpressionResult, BoundScope boundScope)
    {
        return new BoundDiscardStatement(BindExpression(discartExpressionResult.Expression, boundScope));
    }


    public IEnumerable<IBoundStatement> BoundStatements(IEnumerable<IStatement> statements, BoundScope boundScope)
    {
        // yes, we need make it foreach intentionally array because IDE doesn't support yielded return statements while debugging
        var stats = statements.ToArray();
        var bounds = new List<IBoundStatement>(stats.Length);
        foreach (var statement in stats)
        {
            bounds.Add(BindStatement(statement,boundScope));
        }

        return bounds;
    }

    private BoundBlockStatement BindBlockStatement(BlockStatement blockStatement, BoundScope boundScope)
    {
        BoundScope blockBoundScope = boundScope;   
        if (boundScope is not BoundFunctionScope fnScope)
        {
            blockBoundScope = new BoundScope("block-scope", boundScope);
        }
        return new BoundBlockStatement(BoundStatements(blockStatement.Statements,blockBoundScope));
    }

    private BoundReturnStatement BindReturnStatement(ReturnStatement returnStatement, BoundScope boundScope)
    {
        return new BoundReturnStatement(BindExpression(returnStatement.ExpressionSyntax, boundScope));
    }

    private BoundFunctionStatement BindFunctionStatement(FunctionDeclarationStatement functionDeclarationStatement, BoundScope boundScope)
    {
        var functionNameIdentifier = new BoundIdentifier((string)functionDeclarationStatement.Identifier.Value);
        var functionScope = new BoundFunctionScope(functionNameIdentifier.Name, boundScope);
        var paramsDeclaration = BindFunctionParameters(functionDeclarationStatement, functionScope);
        var body = BindStatement(functionDeclarationStatement.Statement,functionScope);
        return new BoundFunctionStatement(
            functionNameIdentifier,
            paramsDeclaration,
            body
        );
    }

    private IEnumerable<BoundParameter> BindFunctionParameters(
        FunctionDeclarationStatement functionDeclarationStatement, BoundFunctionScope scope)
    {
        var parameters = new List<BoundParameter>(functionDeclarationStatement.Parameters.Count());
        foreach (var parameterDeclaration in functionDeclarationStatement.Parameters)
        {
            var type = new BoundIdentifier((string)parameterDeclaration.Type.Value, BoundNodeKind.FunctionParameter);
            var identifier = new BoundIdentifier((string)parameterDeclaration.Identifier.Value, BoundNodeKind.FunctionParameter);
            BoundConstantNumberLiteralExpression? defaultValue = null;
            // if (parameterDeclaration.DefaultValue is not null)
            // {
            //     defaultValue = BindExpression(parameterDeclaration.DefaultValue, scope) as BoundLiteralExpression;
            // }

            var parameter = new BoundParameter(
                type,
                identifier,
                defaultValue
            );
            
            parameters.Add(parameter);
            var typePtr = scope.FindType(type.Name);
            scope.AddParameter(new BoundVariable(identifier.Name, typePtr.Value.Type, scope));
        }

        return parameters;
    }

    public BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax, BoundScope boundScope)
    {
        var boundBinaryOperator =
            BoundBinaryOperator.Bind(binaryExpressionSyntax.Operator.Kind, new Integer32(), new Integer32());

        return new BoundBinaryExpression
        {
            Left = BindExpression(binaryExpressionSyntax.LeftExpression, boundScope),
            Operator = boundBinaryOperator,
            Right = BindExpression(binaryExpressionSyntax.RightExpression, boundScope)
        };
    }

    public IBoundExpression BindExpression(IExpressionSyntax expressionSyntax, BoundScope boundScope)
    {
        return expressionSyntax switch
        {
            BinaryExpressionSyntax binary => BindBinaryExpression(binary, boundScope),
            UnaryExpressionSyntax unary => BindUnaryExpression(unary, boundScope),
            ConstantNumberExpressionSyntax constantNumber => BindLiteralNumber(constantNumber, boundScope),
            ConstantStringExpressionSyntax constantString => BindLiteralString(constantString, boundScope),
            ConstantBooleanExpression constantBoolean => BindLiteralBoolean(constantBoolean, boundScope),
            FunctionCallExpression functionCallExpression => BindFunctionCallExpression(functionCallExpression, boundScope),
            IdentifierExpression nameExpression => BindIdentifierExpression(nameExpression, boundScope),
            _ => throw new NotImplementedException()
        };
    }

    private IBoundExpression BindIdentifierExpression(IdentifierExpression identifierExpression, BoundScope boundScope)
    {
        var identifier = (string) identifierExpression.Identifier.Value;
        var variable = boundScope.FindVariable(identifier);


        return new BoundReadVariableExpression(variable.Value);

    }

    private IBoundExpression BindFunctionCallExpression(FunctionCallExpression functionCallExpression, BoundScope boundScope)
    {
        if (boundScope is not BoundFunctionScope fnScope)
            throw new ArgumentException();
        
        return new BoundFunctionCallExpression(new BoundIdentifier((string)functionCallExpression.Identifier.Value), BindExpressions(functionCallExpression.Arguments, boundScope))
        {
            Type = new Integer32() // TODO: BINDING
        };
    }

    private IEnumerable<IBoundExpression> BindExpressions(IEnumerable<IExpressionSyntax> arguments, BoundScope boundScope)
    {
        var ret = new List<IBoundExpression>();
        foreach (var argument in arguments)
        {
            ret.Add(BindExpression(argument, boundScope));
        }
        return ret;
    }

    private BoundConstantBooleanLiteralExpression BindLiteralBoolean(ConstantBooleanExpression constantBoolean, BoundScope boundScope)
    {
        var boolean = constantBoolean.Bool.Value.ToString()!.Equals("true");
        return new BoundConstantBooleanLiteralExpression(new Bool(boolean));
    }

    private BoundConstantStringExpression BindLiteralString(ConstantStringExpressionSyntax constantString, BoundScope boundScope)
    {
        return new BoundConstantStringExpression("constString", new String(constantString.StringToken.Value.ToString()!));
    }

    private BoundConstantNumberLiteralExpression BindLiteralNumber(ConstantNumberExpressionSyntax constantNumber, BoundScope boundScope)
    {
        return new BoundConstantNumberLiteralExpression(new Integer32((int)constantNumber.NumberToken.Value));
    }

    private BoundUnaryExpression BindUnaryExpression(UnaryExpressionSyntax unary, BoundScope boundScope)
    {
        var boundOperator = BoundUnaryOperator.Bind(unary.Operator.Kind, new Integer32());
        if (boundOperator is null)
        {
            // TODO REPORTING
        }

        var operand = BindExpression(unary.Operand, boundScope);
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