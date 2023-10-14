namespace Djinn.Syntax.Biding.Expressions;

public interface IBoundExpression : IBoundNode
{
    public Type Type { get; }
}