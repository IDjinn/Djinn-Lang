namespace Djinn.Syntax.Biding;

public enum BoundUnaryOperatorKind
{
    Unknown,

    Identity,
    Negation
}

public static class BoundUnaryOperatorKindExtensions
{
    public static BoundUnaryOperatorKind BindUnaryOperatorKind(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => BoundUnaryOperatorKind.Identity,
        SyntaxKind.MinusToken => BoundUnaryOperatorKind.Negation,
        _ => BoundUnaryOperatorKind.Unknown
    };
}