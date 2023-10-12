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

    public IBoundStatement Visit(FunctionStatement functionStatement)
    {
        return BindFunctionStatement(functionStatement);
    }

    public IBoundStatement Visit(ReturnStatement returnStatement)
    {
        return BindReturnStatement(returnStatement);
    }

    public IBoundStatement Visit(BlockStatement blockStatement)
    {
        return BindBlockStatement(blockStatement);
    }

    public IBoundStatement BoundStatement(IStatement statement)
    {
        return statement switch
        {
            FunctionStatement functionStatement => BindFunctionStatement(functionStatement),
            ReturnStatement returnStatement => BindReturnStatement(returnStatement),
            BlockStatement blockStatement => BindBlockStatement(blockStatement),
            _ => throw new NotImplementedException()
        };
    }

    public IEnumerable<IBoundStatement> BoundStatements(IEnumerable<IStatement> statements)
    {
        foreach (var statement in statements)
        {
            yield return BoundStatement(statement);
        }
    }

    private IBoundStatement BindBlockStatement(BlockStatement blockStatement)
    {
        return new BoundBlockStatement(BoundStatements(blockStatement.Statements));
    }

    private IBoundStatement BindReturnStatement(ReturnStatement returnStatement)
    {
        return new BoundReturnStatement(BindExpression(returnStatement.ExpressionSyntax));
    }

    private IBoundStatement BindFunctionStatement(FunctionStatement function)
    {
        return new BoundFunctionStatement(default); // ToDO
    }

    public BoundBinaryExpression BindBinaryExpression(BinaryExpressionSyntax binaryExpressionSyntax)
    {
        return new BoundBinaryExpression()
        {
            Left = BindExpression(binaryExpressionSyntax.LeftExpression),
            OperatorKind = binaryExpressionSyntax.Operator.Kind.BindBinaryOperatorKind(),
            Right = BindExpression(binaryExpressionSyntax.LeftExpression)
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

    private IBoundExpression BindLiteralString(ConstantStringExpressionSyntax constantString)
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

    private IBoundExpression BindLiteralNumber(ConstantNumberExpressionSyntax constantNumber)
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

    private IBoundExpression BindUnaryExpression(UnaryExpressionSyntax unary)
    {
        return new BoundUnaryExpression()
        {
            Operand = BindExpression(unary),
            OperatorKind = unary.Operator.Kind.BindUnaryOperatorKind(),
        };
    }
}