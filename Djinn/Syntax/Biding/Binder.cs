using Djinn.Expressions;
using Djinn.Statements;
using Djinn.Syntax.Biding.Statements;
using Djinn.Utils;

namespace Djinn.Syntax.Biding;

public class Binder : IStatementVisitor<IBoundStatement>, IExpressionVisitor<IBoundExpression>
{
    public IBoundExpression Visit(BinaryExpressionSyntax expressionSyntax)
    {
        return BindBinaryExpression(expressionSyntax);
    }

    public IBoundExpression Visit(ConstantNumberExpressionSyntax expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(ConstantStringExpressionSyntax expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(UnaryExpressionSyntax expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(ParameterExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(NoOpExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(NameExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(FunctionCallExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(ConstantBooleanExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(AssigmentExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundExpression Visit(ArgumentsExpression expressionSyntax)
    {
        return BindExpression(expressionSyntax);
    }

    public IBoundStatement Visit(DiscardExpressionResultStatement discardExpressionResult)
    {
        return BindStatement(discardExpressionResult);
    }

    public IBoundStatement Visit(FunctionStatement functionStatement)
    {
        throw new NotSupportedException();
    }

    public IBoundStatement Visit(ReturnStatement returnStatement)
    {
        return BindReturnStatement(returnStatement);
    }

    public IBoundStatement Visit(BlockStatement blockStatement)
    {
        return BindBlockStatement(blockStatement);
    }

    public IBoundStatement Visit(FunctionDeclarationStatement functionDeclarationStatement)
    {
        return BindFunctionStatement(functionDeclarationStatement);
    }

    public IBoundStatement BindStatement(IStatement statement)
    {
        return statement switch
        {
            ReturnStatement returnStatement => BindReturnStatement(returnStatement),
            BlockStatement blockStatement => BindBlockStatement(blockStatement),
            _ => throw new NotImplementedException()
        };
    }

    public IEnumerable<IBoundStatement> BoundStatements(IEnumerable<IStatement> statements)
    {
        // yes, we need make it foreach intentionally array because IDE doesn't support yielded return statements while debugging
        var stats = statements.ToArray();
        var bounds = new List<IBoundStatement>(stats.Length);
        foreach (var statement in stats)
        {
            bounds.Add(BindStatement(statement));
        }

        return bounds;
    }

    private BoundBlockStatement BindBlockStatement(BlockStatement blockStatement)
    {
        return new BoundBlockStatement(BoundStatements(blockStatement.Statements));
    }

    private BoundReturnStatement BindReturnStatement(ReturnStatement returnStatement)
    {
        return new BoundReturnStatement(BindExpression(returnStatement.ExpressionSyntax));
    }

    private BoundFunctionStatement BindFunctionStatement(FunctionDeclarationStatement functionDeclarationStatement)
    {
        return new BoundFunctionStatement(BindStatement(functionDeclarationStatement.Statement));
    }

    public BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax)
    {
        return new BoundBinaryExpression()
        {
            Left = BindExpression(binaryExpressionSyntax.LeftExpression),
            OperatorKind = binaryExpressionSyntax.Operator.Kind.BindBinaryOperatorKind(),
            Right = BindExpression(binaryExpressionSyntax.RightExpression)
        };
    }

    public IBoundExpression BindExpression(IExpressionSyntax expressionSyntax)
    {
        return expressionSyntax switch
        {
            BinaryExpressionSyntax binary => BindBinaryExpression(binary),
            UnaryExpressionSyntax unary => BindUnaryExpression(unary),
            ConstantNumberExpressionSyntax constantNumber => BindLiteralNumber(constantNumber),
            ConstantStringExpressionSyntax constantString => BindLiteralString(constantString),
            _ => throw new NotImplementedException()
        };
    }

    private BoundLiteralExpression BindLiteralString(ConstantStringExpressionSyntax constantString)
    {
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Value = constantString.StringToken.Value,
                Type = new String(constantString.StringToken.Value.ToString()!)
            }
        };
    }

    private BoundLiteralExpression BindLiteralNumber(ConstantNumberExpressionSyntax constantNumber)
    {
        return new BoundLiteralExpression
        {
            Value = new BoundValue()
            {
                Value = constantNumber.NumberToken.Value,
                Type = new Integer32((int)constantNumber.NumberToken.Value)
            }
        };
    }

    private BoundUnaryExpression BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        return new BoundUnaryExpression()
        {
            Operand = BindExpression(unary),
            OperatorKind = unary.Operator.Kind.BindUnaryOperatorKind(),
        };
    }
}