using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Expressions;

public record UnaryExpressionSyntax(IExpressionSyntax Operand, SyntaxToken Operator) : IExpressionSyntax
{
#if DEBUG
    public string DebugInformationDisplay => $"{Operator.Value} {Operand.DebugInformationDisplay}";
#endif
    public T Accept<T>(IExpressionVisitor<T> expr, BoundScope boundScope)
    {
        return expr.VisitUnaryExpression(this, boundScope);
    }

    public SyntaxKind Kind => SyntaxKind.UnaryExpression;
}