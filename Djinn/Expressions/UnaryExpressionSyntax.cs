using Djinn.Syntax;

namespace Djinn.Expressions;

public record UnaryExpressionSyntax(SyntaxToken Operator, SyntaxKind Kind) : IExpressionSyntax;