using Djinn.Expressions;

namespace Djinn.Utils;

public interface IExpressionVisitor<T> : IVisitor<T>
{
    public T Visit(BinaryExpressionSyntax expressionSyntax);
    public T Visit(ConstantNumberExpressionSyntax expressionSyntax);
    public T Visit(ConstantStringExpressionSyntax expressionSyntax);
}