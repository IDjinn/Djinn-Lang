using Djinn.Utils;

namespace Djinn.SyntaxNodes;

public record SyntaxToken(SyntaxKind Kind, object Value, Position Position)
{
    public static SyntaxToken BadToken = new SyntaxToken(SyntaxKind.BadToken, new object(), new Position());
    public static SyntaxToken Void = new SyntaxToken(SyntaxKind.Void, new object(), new Position());
}