using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

[System.Diagnostics.DebuggerDisplay("{DebugInformationDisplay}")]
public record FunctionDeclarationStatement(
    SyntaxToken ReturnType,
    SyntaxToken Identifier,
    IEnumerable<ParameterDeclaration> Parameters,
    IStatement Statement // TODO BETTER TYPING THIS
) : IStatement
{
#if DEBUG
    public string DebugInformationDisplay =>
        $"fn {ReturnType.Value} {Identifier.Value} ({string.Join(", ", Parameters.Select(param => param.DebugInformationDisplay))})";
#endif

    public SyntaxKind Kind => SyntaxKind.FunctionDeclaration;


    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }

    // public T Generate<T>(IStatementVisitor<T> visitor, CodeGen codeGen)
    // {
    //     var paramList = Parameters.Parameters.ToArray();
    //     var args = new LLVMTypeRef[paramList.Length];
    //     for (var i = 0; i < paramList.Length; i++)
    //     {
    //         var keyword = KeywordExtensions.FromString(paramList[i].DefaultValue!.Value.ToString()!);
    //         var type = keyword.ToLLVMType();
    //         args[i] = type;
    //     }
    //
    //     var retType = KeywordExtensions.FromString(ReturnType.Value.ToString()!).ToLLVMType();
    //     var functionType = LLVM.FunctionType(retType, args, new LLVMBool(0));
    //     var function = LLVM.AddFunction(codeGen.Module, Identifier.Value.ToString(), functionType);
    //     for (var i = 0; i < paramList.Length; i++)
    //     {
    //         var argument = paramList[i];
    //         var identifier = argument.Identifier.Value.ToString();
    //         var param = LLVM.GetParam(function, (uint)i);
    //         LLVM.SetValueName(param, identifier);
    //     }
    //
    //     codeGen.Stack.Push(new Tuple<LLVMValueRef, object>(function, Statement));
    //     return Statement.Generate(visitor, codeGen);
    // }
}