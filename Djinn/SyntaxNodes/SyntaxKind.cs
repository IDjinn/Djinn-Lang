namespace Djinn.SyntaxNodes;

public enum SyntaxKind
{
    BadToken = 0,
    EndOfFileToken,

    
    NumberLiteral,
    
    
    WhiteSpaceToken,
    
    PlusToken,
    MinusToken,
    SlashToken,
    StarToken,
    Identifier,
    OpenParenthesis,
    CloseParenthesis,
    ConstantNumberExpression,
    BinaryExpression,
    
    
    VariableDeclaration,
    VariableAssignment,
    FunctionDeclaration,
    EqualsOperator,
    
    Void,
    Int32,
    OpenBrace,
    CloseBrace,
    ReturnDeclaration,
    FunctionCallExpression,
    StringLiteral,
    ConstantStringExpression,
    BlockStatement
}

public static class SyntaxKindExtensions
{
    public static bool IsLogicOperator(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => true,
        SyntaxKind.MinusToken => true,
        SyntaxKind.SlashToken => true,
        SyntaxKind.StarToken => true,
        _ => false,
    };

    public static bool IsValueType(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.Void or SyntaxKind.Int32 => true,
        _ => false,
    };
}