using Djinn.Compile;
using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;
using LLVMSharp;

namespace Djinn.Statements;

public record ReturnStatement(IExpressionSyntax ExpressionSyntax) : IStatement
{
    public static ReturnStatement Void = new(new NoOpExpression());
    public SyntaxKind Kind => SyntaxKind.ReturnDeclaration;
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }

}