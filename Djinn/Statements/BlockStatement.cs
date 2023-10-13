using Djinn.Compile;
using Djinn.Syntax;
using LLVMSharp;

namespace Djinn.Statements;

public record BlockStatement : IStatement
{
    private readonly List<IStatement> _statements = new();

    public BlockStatement(IEnumerable<IStatement> statements)
    {
        _statements.AddRange(statements);
    }

    public IEnumerable<IStatement> Statements => _statements.ToArray();
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

        // TODO MOVE FROM HERE
        if (Statements.All(x => x.Kind != SyntaxKind.ReturnDeclaration))
        {
            _statements.Add(ReturnStatement.Void);
        }

        foreach (var statement in Statements)
        {
            statement.Generate(visitor, codeGen);
        }


        return default;
    }
}