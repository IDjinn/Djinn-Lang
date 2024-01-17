using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record ImportStatement(
    SyntaxToken Keyword,
    SyntaxToken Library
) : IStatement
{
#if DEBUG
    public string DebugInformationDisplay => $"import {Library.Value}";
#endif

    public SyntaxKind Kind => SyntaxKind.ImportStatement;

    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}