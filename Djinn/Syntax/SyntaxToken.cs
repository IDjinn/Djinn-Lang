using Djinn.Utils;

namespace Djinn.Syntax;

public record SyntaxToken(SyntaxKind Kind, object Value, Position Position)
{
    public static SyntaxToken BadToken = new SyntaxToken(SyntaxKind.BadToken, new object(), new Position());
    public static SyntaxToken Unknown = new SyntaxToken(SyntaxKind.Unknown, new object(), new Position());
    public static SyntaxToken Void = new SyntaxToken(SyntaxKind.Void, new object(), new Position());
    public static SyntaxToken EndOfFileToken = new SyntaxToken(SyntaxKind.EndOfFileToken, new object(), new Position());

    public static SyntaxToken FromIdentifier(SyntaxKind kind, object value) =>
        new SyntaxToken(kind, value, new Position());
}