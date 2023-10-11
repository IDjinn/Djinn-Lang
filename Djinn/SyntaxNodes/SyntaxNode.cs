using Djinn.Utils;

namespace Djinn.SyntaxNodes;

public interface ISyntaxNode
{
    public SyntaxKind Kind { get; }
}