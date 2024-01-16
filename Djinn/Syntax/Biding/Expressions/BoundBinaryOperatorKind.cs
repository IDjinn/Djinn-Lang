using LLVMSharp;

namespace Djinn.Syntax.Biding.Expressions;

public enum BoundBinaryOperatorKind
{
    Unknown,

    Addition,
    Subtraction,
    Multiplication,
    Division,
    LogicalAnd,
    LogicalOr,
    Equals,
    NotEquals,
    
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
}

public static class BoundBinaryOperatorExtensions
{
    public static LLVMIntPredicate ToLLVMIntPredicate(this BoundBinaryOperatorKind @operator) => @operator switch
    {
        BoundBinaryOperatorKind.Equals => LLVMIntPredicate.LLVMIntEQ,
        BoundBinaryOperatorKind.NotEquals => LLVMIntPredicate.LLVMIntNE,
        BoundBinaryOperatorKind.LessThan => LLVMIntPredicate.LLVMIntULT,
        BoundBinaryOperatorKind.LessThanEqual => LLVMIntPredicate.LLVMIntULE,
        BoundBinaryOperatorKind.GreaterThan => LLVMIntPredicate.LLVMIntSGT,
        BoundBinaryOperatorKind.GreaterThanEqual => LLVMIntPredicate.LLVMIntSLE,
        _ => throw new ArgumentOutOfRangeException(nameof(@operator), @operator, null)
    };
    
    public static BoundBinaryOperatorKind BindBinaryOperatorKind(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.PlusToken => BoundBinaryOperatorKind.Addition,
        SyntaxKind.MinusToken => BoundBinaryOperatorKind.Subtraction,
        SyntaxKind.StarToken => BoundBinaryOperatorKind.Multiplication,
        SyntaxKind.SlashToken => BoundBinaryOperatorKind.Division,
        SyntaxKind.LessThanOperator => BoundBinaryOperatorKind.LessThan, 
        SyntaxKind.LessThanEqualsOperator => BoundBinaryOperatorKind.LessThanEqual,
        SyntaxKind.GreaterThanOperator => BoundBinaryOperatorKind.GreaterThan,
        SyntaxKind.GreaterThanEqualsOperator => BoundBinaryOperatorKind.GreaterThanEqual,
        _ => BoundBinaryOperatorKind.Unknown
    };
}