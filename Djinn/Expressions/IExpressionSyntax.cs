using Djinn.Syntax;
using Djinn.Utils;

namespace Djinn.Expressions;

public interface IExpressionSyntax : ISyntaxNode
{
    T Accept<T>(IExpressionVisitor<T> expr);
}