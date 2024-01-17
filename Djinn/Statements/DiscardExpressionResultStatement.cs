using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record DiscardExpressionResultStatement(IExpressionSyntax Expression) : IStatement
{
#if DEBUG
    public string DebugInformationDisplay => $"_ => {Expression.DebugInformationDisplay}";
#endif

    public SyntaxKind Kind => SyntaxKind.BlockStatement;


    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}