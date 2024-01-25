using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record ReturnStatement(IExpressionSyntax? ExpressionSyntax) : IStatement
{
    public static ReturnStatement Void = new(new NoOpExpression());

#if DEBUG
    public string DebugInformationDisplay => $"ret {ExpressionSyntax?.DebugInformationDisplay}";
#endif
    public SyntaxKind Kind => SyntaxKind.Return;


    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}