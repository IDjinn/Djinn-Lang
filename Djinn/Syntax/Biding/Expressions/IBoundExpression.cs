namespace Djinn.Syntax.Biding;

public interface IBoundExpression : IBoundNode
{
    public IType Type { get; }
}