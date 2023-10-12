namespace Djinn.Syntax.Biding;

public abstract class BoundExpression : BoundNode
{
    public abstract IType Type { get; }
}