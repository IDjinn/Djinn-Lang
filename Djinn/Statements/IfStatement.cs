using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record IfStatement(
    IExpressionSyntax Conditional, // TODO MAKE THIS CONDITIONAL AT COMPILE TIME
    IStatement Block,
    IStatement? Else
) : IStatement
{
#if DEBUG
    public string DebugInformationDisplay =>
        $"if {Conditional.DebugInformationDisplay} ({Block.DebugInformationDisplay})";
#endif

    public SyntaxKind Kind => SyntaxKind.If;

    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}