namespace Djinn.Syntax.Biding.Expressions;

public enum BoundUnaryOperatorKind
{
    Unknown,

    Addition,
    Subtraction,
    Multiplication,
    Division,
}

public static class BoundUnaryOperatorKindExtensions
{
    public static BoundUnaryOperatorKind BindUnaryOperatorKind(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => BoundUnaryOperatorKind.Addition,
        SyntaxKind.MinusToken => BoundUnaryOperatorKind.Subtraction,
        _ => BoundUnaryOperatorKind.Unknown
    };
}