using Djinn.Statements;
using Djinn.Syntax.Biding.Scopes;
using Djinn.Utils;

namespace Djinn.Syntax;

public record SyntaxTree
{
    public required IReadOnlyList<IStatement> Statements { get; init; }
    public required IReadOnlyList<Diagnostic> Diagnostics { get; init; }

    public IEnumerable<T> Generate<T>(IStatementVisitor<T> statementVisitor)
    {
        var globalScope = new Scope("global");
        var generated = new List<T>();
        foreach (var syntaxNode in Statements)
        {
            generated.Add(syntaxNode.Visit(statementVisitor, globalScope));
        }

        return generated;
    }
}