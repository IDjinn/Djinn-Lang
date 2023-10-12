using Djinn.Expressions;
using Djinn.Syntax;

namespace Djinn.Statements;

public readonly record struct ReturnStatement(SyntaxToken Type, IExpressionSyntax ExpressionSyntax) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.ReturnDeclaration;

    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Generate<T>(IStatementVisitor<T> visitor)
    {
        return default;
    }
}