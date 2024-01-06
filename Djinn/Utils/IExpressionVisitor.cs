using Djinn.Expressions;

namespace Djinn.Utils;

public interface IExpressionVisitor<T> : IVisitor<T>
{
    public T Visit(BinaryExpressionSyntax expressionSyntax);
    public T Visit(ConstantNumberExpressionSyntax expressionSyntax);
    public T Visit(ConstantStringExpressionSyntax expressionSyntax);
    public T Visit(UnaryExpressionSyntax expressionSyntax);
    public T Visit(ParameterDeclaration declarationSyntax);
    public T Visit(NoOpExpression expressionSyntax);
    public T Visit(NameExpression expressionSyntax);
    public T Visit(FunctionCallExpression expressionSyntax);
    public T Visit(ConstantBooleanExpression expressionSyntax);
    public T Visit(AssigmentExpression expressionSyntax);
    public T Visit(ArgumentsExpression expressionSyntax);
}