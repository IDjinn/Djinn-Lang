using Djinn.Compile;
using Djinn.Expressions;
using Djinn.Syntax;

namespace Djinn.Statements;

public record ParametersDeclarationStatement(IEnumerable<ParameterExpression> Parameters) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.FunctionParametersExpression;

    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        throw new NotImplementedException();
    }

    public T Generate<T>(IStatementVisitor<T> visitor, CodeGen codeGen)
    {
        throw new NotImplementedException();
    }
}