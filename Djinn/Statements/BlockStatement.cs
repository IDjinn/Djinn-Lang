using Djinn.Syntax;

namespace Djinn.Statements;

public record BlockStatement(IEnumerable<IStatement> Statements) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.BlockStatement;


    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Generate<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}