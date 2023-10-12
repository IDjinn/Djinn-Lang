using Djinn.Syntax;

namespace Djinn.Statements;

public readonly record struct BlockStatement(IEnumerable<IStatement> Statements) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.BlockStatement;


    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }

    public T Generate<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}