using Djinn.Syntax;

namespace Djinn.Expressions;

public interface IExpressionSyntax : ISyntaxNode
{
    public SyntaxToken ReturnType { get; }
}