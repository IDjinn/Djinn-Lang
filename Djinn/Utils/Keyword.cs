using Djinn.Syntax;

namespace Djinn.Utils;

public enum Keyword
{
    Unknown,

    This,
    Base,
    New,

    For,
    While,
    Do,
    If,
    Else,
    Try,
    Catch,
    Finally,

    Integer1,
    Integer8,
    Integer16,
    Integer32,
    Integer64,
    Integer128,

    Float16,
    Float32,
    Float64,
    Float80,
    Float128,

    Bool,
    True,
    False,
    Null,
    Void,
    String,

    Return,
    Struct,
    Class,
    Record,
    Function,
}

public static class KeywordExtensions
{
    public static Keyword FromString(string str)
    {
        return str switch
        {
            "void" => Keyword.Void,
            "function" or "fn" => Keyword.Function,
            "ret" or "return" => Keyword.Return,
            _ => Keyword.Unknown
        };
    }

    public static SyntaxKind ToTokenKind(Keyword keyword)
    {
        return keyword switch
        {
            Keyword.Void => SyntaxKind.Void,
            Keyword.Function => SyntaxKind.FunctionDeclaration,
            Keyword.Return => SyntaxKind.ReturnDeclaration,
            _ => SyntaxKind.BadToken
        };
    }
}