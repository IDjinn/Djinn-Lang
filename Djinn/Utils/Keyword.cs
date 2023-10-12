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
            "true" => Keyword.True,
            "false" => Keyword.False,
            "null" => Keyword.Null,
            "string" => Keyword.String,
            "int1" => Keyword.Integer1,
            "int8" or "byte" => Keyword.Integer8,
            "int16" or "short" => Keyword.Integer16,
            "int32" or "int" => Keyword.Integer32,
            "int64" or "long" => Keyword.Integer64,
            "int128" => Keyword.Integer128,
            "float16" or "float" => Keyword.Float16,
            "float32" or "double" => Keyword.Float32,
            "float64" => Keyword.Float64,
            "float80" => Keyword.Float80,
            "float128" or "decimal" => Keyword.Float128,
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

            Keyword.True => SyntaxKind.True,
            Keyword.False => SyntaxKind.False,

            Keyword.Float16 => SyntaxKind.Float16,
            Keyword.Float32 => SyntaxKind.Float32,
            Keyword.Float64 => SyntaxKind.Float64,
            Keyword.Float80 => SyntaxKind.Float80,
            Keyword.Float128 => SyntaxKind.Float128,

            Keyword.Integer1 => SyntaxKind.Integer1,
            Keyword.Integer8 => SyntaxKind.Integer8,
            Keyword.Integer16 => SyntaxKind.Integer16,
            Keyword.Integer32 => SyntaxKind.Integer32,
            Keyword.Integer64 => SyntaxKind.Integer64,
            Keyword.Integer128 => SyntaxKind.Integer128,

            Keyword.String => SyntaxKind.String,
            Keyword.Null => SyntaxKind.Null,

            _ => SyntaxKind.BadToken
        };
    }
}