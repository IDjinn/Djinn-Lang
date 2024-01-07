using Djinn.Compile;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record FunctionStatement(SyntaxToken Type) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.FunctionDeclaration;
    public T Visit<T>(IStatementVisitor<T> visitor, Scope scope)
    {
        return visitor.Visit(this, scope);
    }

}