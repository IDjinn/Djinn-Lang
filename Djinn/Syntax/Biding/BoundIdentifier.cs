namespace Djinn.Syntax.Biding;

public record BoundIdentifier(string Name, BoundNodeKind Kind = BoundNodeKind.FunctionIdentifier) : IBoundNode;