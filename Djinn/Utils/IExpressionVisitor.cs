using Djinn.Expressions;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Utils;

public interface IExpressionVisitor<T> : IVisitor<T>
{
    public T VisitBinaryExpression(BinaryExpressionSyntax expressionSyntax, Scope scope);
    public T VisitConstantNumberExpression(ConstantNumberExpressionSyntax expressionSyntax, Scope scope);
    public T VisitConstantStringExpression(ConstantStringExpressionSyntax expressionSyntax, Scope scope);
    public T VisitUnaryExpression(UnaryExpressionSyntax expressionSyntax, Scope scope);
    public T VisitParameterDeclaration(ParameterDeclaration declarationSyntax, Scope scope);
    public T VisitNoOpExpression(NoOpExpression expressionSyntax, Scope scope);
    public T VisitIdentifierExpression(IdentifierExpression expressionSyntax, Scope scope);
    public T VisitFunctionCallExpression(FunctionCallExpression expressionSyntax, Scope scope);
    public T VisitConstantBooleanExpression(ConstantBooleanExpression expressionSyntax, Scope scope);
    public T VisitAssigmentExpression(AssigmentExpression expressionSyntax, Scope scope);
    public T VisitArgumentsExpression(ArgumentsExpression expressionSyntax, Scope scope);
}