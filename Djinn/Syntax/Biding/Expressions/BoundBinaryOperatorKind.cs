namespace Djinn.Syntax.Biding.Expressions;

public enum BoundBinaryOperatorKind
{
    Unknown,

    Addiction,
    Subtraction,
    Multiplication,
    Division
}

public static class BoundBinaryOperatorExtensions
{
    public static BoundBinaryOperatorKind BindBinaryOperatorKind(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => BoundBinaryOperatorKind.Addiction,
        SyntaxKind.MinusToken => BoundBinaryOperatorKind.Subtraction,
        SyntaxKind.StarToken => BoundBinaryOperatorKind.Multiplication,
        SyntaxKind.SlashToken => BoundBinaryOperatorKind.Division,
        _ => BoundBinaryOperatorKind.Unknown
    };
}