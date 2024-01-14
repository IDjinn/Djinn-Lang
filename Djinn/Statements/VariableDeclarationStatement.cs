using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record VariableDeclarationStatement(SyntaxToken Type, SyntaxToken Identifier, IExpressionSyntax Expression) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.LocalVariableDeclaration;
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}