using Djinn.Compile;
using Djinn.Syntax;
using LLVMSharp;

namespace Djinn.Statements;

public record BlockStatement(IEnumerable<IStatement> Statements) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.BlockStatement;


    public T Visit<T>(IStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public T Generate<T>(IStatementVisitor<T> visitor, CodeGen codeGen)
    {
        if (codeGen.Stack.Any())
        {
            var block = LLVM.AppendBasicBlock(codeGen.Stack.Pop().Item1, "entry");
            LLVM.PositionBuilderAtEnd(codeGen.Builder, block);
        }

        foreach (var statement in Statements)
        {
            statement.Generate(visitor, codeGen);
        }

        return default;
    }
}