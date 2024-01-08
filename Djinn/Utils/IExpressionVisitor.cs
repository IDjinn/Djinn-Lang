using Djinn.Expressions;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Utils;

public interface IExpressionVisitor<T> : IVisitor<T>
{
    public T VisitBinaryExpression(BinaryExpressionSyntax expressionSyntax, BoundScope boundScope);
    public T VisitConstantNumberExpression(ConstantNumberExpressionSyntax expressionSyntax, BoundScope boundScope);
    public T VisitConstantStringExpression(ConstantStringExpressionSyntax expressionSyntax, BoundScope boundScope);
    public T VisitUnaryExpression(UnaryExpressionSyntax expressionSyntax, BoundScope boundScope);
    public T VisitParameterDeclaration(ParameterDeclaration declarationSyntax, BoundScope boundScope);
    public T VisitNoOpExpression(NoOpExpression expressionSyntax, BoundScope boundScope);
    public T VisitIdentifierExpression(IdentifierExpression expressionSyntax, BoundScope boundScope);
    public T VisitFunctionCallExpression(FunctionCallExpression expressionSyntax, BoundScope boundScope);
    public T VisitConstantBooleanExpression(ConstantBooleanExpression expressionSyntax, BoundScope boundScope);
    public T VisitAssigmentExpression(AssigmentExpression expressionSyntax, BoundScope boundScope);
    public T VisitArgumentsExpression(ArgumentsExpression expressionSyntax, BoundScope boundScope);
}