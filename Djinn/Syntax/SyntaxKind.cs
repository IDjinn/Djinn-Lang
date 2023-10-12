namespace Djinn.Syntax;

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
    BlockStatement,
    IncrementOperator,
    PlusAssignmentOperator,
    DecrementOperator,
    MinusAssignmentOperator,
    EqualsEqualsOperator
}

public static class SyntaxKindExtensions
{
    public static bool IsMathOperator(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => true,
        SyntaxKind.MinusToken => true,
        SyntaxKind.SlashToken => true,
        SyntaxKind.StarToken => true,
        SyntaxKind.IncrementOperator => true,
        SyntaxKind.PlusAssignmentOperator => true,
        SyntaxKind.DecrementOperator => true,
        SyntaxKind.MinusAssignmentOperator => true,
        _ => false,
    };

    public static bool IsLogicOperator(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => true,
        SyntaxKind.MinusToken => true,
        SyntaxKind.SlashToken => true,
        SyntaxKind.StarToken => true,
        SyntaxKind.IncrementOperator => true,
        SyntaxKind.PlusAssignmentOperator => true,
        SyntaxKind.DecrementOperator => true,
        SyntaxKind.MinusAssignmentOperator => true,
        SyntaxKind.EqualsEqualsOperator => true,
        _ => false,
    };

    public static bool IsValueType(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.Void or SyntaxKind.Int32 => true,
        _ => false,
    };
}