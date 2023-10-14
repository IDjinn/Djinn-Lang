namespace Djinn.Syntax.Biding.Expressions;

public enum BoundBinaryOperatorKind
{
    Unknown,

    Addition,
    Subtraction,
    Multiplication,
    Division,
    LogicalAnd,
    LogicalOr,
    Equals,
    NotEquals
}

public static class BoundBinaryOperatorExtensions
{
    public static BoundBinaryOperatorKind BindBinaryOperatorKind(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => BoundBinaryOperatorKind.Addition,
        SyntaxKind.MinusToken => BoundBinaryOperatorKind.Subtraction,
        SyntaxKind.StarToken => BoundBinaryOperatorKind.Multiplication,
        SyntaxKind.SlashToken => BoundBinaryOperatorKind.Division,
        _ => BoundBinaryOperatorKind.Unknown
    };
}