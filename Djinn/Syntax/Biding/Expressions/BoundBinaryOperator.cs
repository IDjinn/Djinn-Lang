namespace Djinn.Syntax.Biding.Expressions;

public record BoundBinaryOperator(
    SyntaxKind SyntaxKind,
    BoundBinaryOperatorKind OperatorKind,
    IType OperandType,
    IType ResultType
)
{
    public static BoundBinaryOperator[] Operators =
    {
        new(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, new Integer32()),
        new(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, new Integer32()),
        new(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, new Integer32()),
        new(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, new Integer32()),
        new(SyntaxKind.EqualsEqualsOperator, BoundBinaryOperatorKind.Equals, new Integer32(), new Bool()),
        new(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, new Integer32(), new Bool()),

        new(SyntaxKind.AmpersendAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, new Bool()),
        new(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, new Bool()),
        new(SyntaxKind.EqualsEqualsOperator, BoundBinaryOperatorKind.Equals, new Bool()),
        new(SyntaxKind.BangEqualsToken, BoundBinaryOperatorKind.NotEquals, new Bool()),
    };

    public BoundBinaryOperator(SyntaxKind SyntaxKind, BoundBinaryOperatorKind OperatorKind, IType OperandType) :
        this(SyntaxKind, OperatorKind, OperandType, OperandType)
    {
    }

    public IType ResultType { get; init; } = ResultType;

    public static BoundBinaryOperator? Bind(SyntaxKind kind, IType leftType, IType rightType)
    {
        foreach (var binaryOperator in Operators)
        {
            if (binaryOperator.SyntaxKind == kind
                && binaryOperator.OperandType.GetType().IsInstanceOfType(leftType)
                && binaryOperator.OperandType.GetType().IsInstanceOfType(rightType)
               )
                return binaryOperator;
        }

        return null;
    }
}