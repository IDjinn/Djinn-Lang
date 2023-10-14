namespace Djinn.Syntax.Biding.Expressions;

public record BoundBinaryOperator(
    SyntaxKind SyntaxKind,
    BoundBinaryOperatorKind Operator,
    Type OperandType,
    Type ResultType
)
{
    public static BoundBinaryOperator[] Operators =
    {
        new(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(INumber)),
        new(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(INumber)),
        new(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(INumber)),
        new(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(INumber)),
        new(SyntaxKind.EqualsEqualsOperator, BoundBinaryOperatorKind.Equals, typeof(INumber), typeof(Bool)),
        new(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(INumber), typeof(Bool)),

        new(SyntaxKind.AmpersendAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(Bool)),
        new(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(Bool)),
        new(SyntaxKind.EqualsEqualsOperator, BoundBinaryOperatorKind.Equals, typeof(Bool)),
        new(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(Bool)),
    };

    public BoundBinaryOperator(SyntaxKind SyntaxKind, BoundBinaryOperatorKind Operator, Type OperandType) :
        this(SyntaxKind, Operator, OperandType, OperandType)
    {
    }

    public Type ResultType { get; init; } = ResultType;

    public static BoundBinaryOperator? Bind(SyntaxKind kind, IType leftType, IType rightType)
    {
        foreach (var binaryOperator in Operators)
        {
            if (binaryOperator.SyntaxKind == kind
                && binaryOperator.OperandType.IsInstanceOfType(leftType)
                && binaryOperator.OperandType.IsInstanceOfType(rightType)
               )
                return binaryOperator;
        }

        return null;
    }
}