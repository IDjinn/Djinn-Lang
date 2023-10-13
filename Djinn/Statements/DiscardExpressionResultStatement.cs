using Djinn.Compile;
using Djinn.Expressions;
using Djinn.Syntax;

namespace Djinn.Statements;

public record DiscardExpressionResultStatement(IExpressionSyntax Expression) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.BlockStatement;

    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Generate<T>(IStatementVisitor<T> visitor, CodeGen codeGen)
    {
        Expression.Accept(codeGen);
        return default; // TODO: Implement 
    }
}