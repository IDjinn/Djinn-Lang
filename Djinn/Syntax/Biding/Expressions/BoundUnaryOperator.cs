namespace Djinn.Syntax.Biding.Expressions;

public record BoundUnaryOperator
{
    public static BoundUnaryOperator[] Operators = new BoundUnaryOperator[]
    {
        new()
        {
            SyntaxKind = SyntaxKind.BangToken,
            Operator = BoundUnaryOperatorKind.LogicalNegation,
            OperandType = typeof(Bool),
            ResultType = typeof(Bool)
        },
        new()
        {
            SyntaxKind = SyntaxKind.PlusToken,
            Operator = BoundUnaryOperatorKind.Identity,
            OperandType = typeof(INumber),
            ResultType = typeof(INumber)
        },
        new()
        {
            SyntaxKind = SyntaxKind.MinusToken,
            Operator = BoundUnaryOperatorKind.Negation,
            OperandType = typeof(INumber),
            ResultType = typeof(INumber)
        },
    };

    public required SyntaxKind SyntaxKind { get; init; }
    public required BoundUnaryOperatorKind Operator { get; init; }
    public required Type OperandType { get; init; }
    public required Type ResultType { get; init; }

    public static BoundUnaryOperator? Bind(SyntaxKind kind, IType operandType)
    {
        foreach (var unaryOperator in Operators)
        {
            if (unaryOperator.SyntaxKind == kind && unaryOperator.OperandType.IsInstanceOfType(operandType))
                return unaryOperator;
        }

        return null;
    }
}