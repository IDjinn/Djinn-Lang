using Djinn.Statements;
using Djinn.Utils;

namespace Djinn.Syntax;

public readonly record struct SyntaxTree
{
    public required IReadOnlyList<IStatement> Statements { get; init; }
    public required IReadOnlyList<Diagnostic> Diagnostics { get; init; }

    public IEnumerable<T> Generate<T>(IStatementVisitor<T> statementVisitor)
    {
        foreach (var syntaxNode in Statements)
        {
            yield return syntaxNode.Visit(statementVisitor);
        }
    }
}