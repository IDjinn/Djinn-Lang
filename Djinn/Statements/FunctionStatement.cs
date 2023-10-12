using Djinn.Syntax;

namespace Djinn.Statements;

public record FunctionStatement(SyntaxToken Type) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.FunctionDeclaration;

    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Generate<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}