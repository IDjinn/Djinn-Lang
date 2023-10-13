using Djinn.Compile;
using Djinn.Expressions;
using Djinn.Syntax;
using LLVMSharp;

namespace Djinn.Statements;

public record ReturnStatement(IExpressionSyntax ExpressionSyntax) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.ReturnDeclaration;

    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Generate<T>(IStatementVisitor<T> visitor, CodeGen codeGen)
    {
        var expressionValue = ExpressionSyntax.Accept(codeGen);
        LLVM.BuildRet(codeGen.Builder, (LLVMValueRef)expressionValue);
        return default;
    }
}