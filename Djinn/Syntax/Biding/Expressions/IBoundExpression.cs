namespace Djinn.Syntax.Biding.Expressions;

public interface IBoundExpression : IBoundNode
{
    public IType Type { get; }
}