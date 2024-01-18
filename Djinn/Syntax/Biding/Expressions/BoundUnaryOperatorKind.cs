namespace Djinn.Syntax.Biding.Expressions;

public enum BoundUnaryOperatorKind
{
    Unknown,
    Identity,
    Negation,
    LogicalNegation,

    Addition
}

public static class BoundUnaryOperatorKindExtensions
{
    public static BoundUnaryOperatorKind BindUnaryOperatorKind(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => BoundUnaryOperatorKind.Identity,
        SyntaxKind.MinusToken => BoundUnaryOperatorKind.Negation,
        _ => BoundUnaryOperatorKind.Unknown
    };

    public static string UnaryOperatorKindToString(this BoundUnaryOperatorKind kind) => kind switch
    {
        BoundUnaryOperatorKind.Identity => "+",
        BoundUnaryOperatorKind.Negation => "-",
        BoundUnaryOperatorKind.LogicalNegation => "!"
    };
}