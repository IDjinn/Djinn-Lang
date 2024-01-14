using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record ImportStatement(
    SyntaxToken Keyword,
    SyntaxToken Library
    ) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.ImportStatement;
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}