using Djinn.Syntax;

namespace Djinn.Statements;

public record FunctionDeclarationStatement(
    SyntaxToken Identifier,
    ParametersDeclarationStatement Parameters,
    IStatement Statement // TODO BETTER TYPING THIS
) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.FunctionDeclaration;

    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }

    public T Generate<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }
}