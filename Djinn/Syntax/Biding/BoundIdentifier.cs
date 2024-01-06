namespace Djinn.Syntax.Biding;

public record BoundIdentifier(string Name) : IBoundNode
{
    public BoundNodeKind Kind => BoundNodeKind.FunctionIdentifier;
}