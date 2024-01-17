using System.Diagnostics;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

[DebuggerDisplay("{DisplayString}")]
public record BlockStatement : IStatement
{
    private readonly List<IStatement> _statements = new();

    public BlockStatement(IEnumerable<IStatement> statements)
    {
        _statements.AddRange(statements);
    }

    public IEnumerable<IStatement> Statements => _statements.ToArray();
    public static BlockStatement Empty => new BlockStatement(new IStatement[] { });
    public SyntaxKind Kind => SyntaxKind.BlockStatement;


    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
#if DEBUG
    public string DisplayString =>
        $"({_statements.Count}) => [{string.Join(", ", _statements.Select(stat => stat.DebugInformationDisplay))}]";

    public string DebugInformationDisplay => $"({_statements.Count}) statements";

#endif
}