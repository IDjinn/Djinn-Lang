using Microsoft.CodeAnalysis;

namespace Djinn.Syntax;

[Flags]
public enum SyntaxKind : long
{
    Unknown = 0,
    Trivia = 1 << 2,
    LogicalOperators = 1 << 4,
    ArithmeticOperators = LogicalOperators | 1 << 5,
    Assignment = 1 << 6,
    Declaration = 1 << 8,
    Expression = 1 << 10,
    Identifier = 1 << 12,
    Variable = 1 << 16,
    Type = 1 << 17,
    Function = 1 << 18,
    Block = 1 << 20,

    ValueTypes = 1 << 22 | Type,

    Float = 1 << 24 | ValueTypes,
    Integer = 1 << 26 | ValueTypes,
    String = 1 << 28 | ValueTypes,
    Null = 1 << 30 | ValueTypes,
    Constant = 1 << 31 | ValueTypes,

    BadToken = Trivia | 1,
    EndOfFileToken = Trivia | 2,
    WhiteSpaceToken = Trivia | 3,

    PlusToken = LogicalOperators | ArithmeticOperators | 1,
    MinusToken = LogicalOperators | ArithmeticOperators | 2,
    SlashToken = LogicalOperators | ArithmeticOperators | 3,
    StarToken = LogicalOperators | ArithmeticOperators | 4,
    EqualsOperator = LogicalOperators | 5,
    IncrementOperator = LogicalOperators | ArithmeticOperators | 6,
    PlusAssignmentOperator = LogicalOperators | ArithmeticOperators | 7,
    DecrementOperator = LogicalOperators | ArithmeticOperators | 8,
    MinusAssignmentOperator = LogicalOperators | ArithmeticOperators | 9,
    EqualsEqualsOperator = LogicalOperators | 9,

    FunctionIdentifier = Identifier | 1,
    LambdaIdentifier = Identifier | 2,
    ClassIdentifier = Identifier | 3,
    NamespaceIdentifier = Identifier | 4,
    LocalVariableIdentifier = Identifier | 5,
    ParamVariableIdentifier = Identifier | 6,

    BinaryExpression = Expression | 1,
    UnaryExpression = Expression | 2,
    VariableAssignmentExpression = Variable | Expression | 1,
    FunctionCallExpression = Function | Expression | 1,
    FunctionParametersExpression = Function | Expression | 2,
    FunctionArgumentsExpression = Function | Expression | 3,

    ParameterVariableDeclaration = Variable | Declaration | 1,
    LocalVariableDeclaration = Variable | Declaration | 2,
    FunctionDeclaration = Function | Declaration | 3,

    OpenBrace = Block | 1,
    CloseBrace = Block | 2,
    ReturnDeclaration = Block | 3,
    BlockStatement = Block | 4,
    OpenParenthesis = Block | 5,
    CloseParenthesis = Block | 6,

    FloatLiteral = Constant | Float | 1,
    NumberLiteral = Constant | Integer | 1,
    StringLiteral = Constant | String | 1,

    ConstantStringExpression = Variable | NumberLiteral | Expression | 1,
    ConstantNumberExpression = Variable | StringLiteral | Expression | 1,
    ConstantFloatExpression = Variable | FloatLiteral | Expression | 1,

    Void = ValueTypes | 30,

    True = ValueTypes | 2,
    False = ValueTypes | 3,

    StringType = ValueTypes | String | 1,
    NullType = ValueTypes | Null | 2,

    Float16 = Float | 1,
    Float32 = Float | 2,
    Float64 = Float | 3,
    Float80 = Float | 4,
    Float128 = Float | 5,

    Integer1 = Integer | 1,
    Integer8 = Integer | 2,
    Integer16 = Integer | 3,
    Integer32 = Integer | 4,
    Integer64 = Integer | 5,
    Integer128 = Integer | 6,
    BangToken,
    Ampersend,
    AmpersendAmpersandToken,
    PipeToken,
    PipePipeToken,
    BangEqualsToken,
    IfStatement,
    ImportStatement,
    Import,
    Switch,
    Case,
    Default
}

public static class SyntaxKindExtensions
{
    public const int InvalidOperatorPrecedence = -1;

    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.AmpersendAmpersandToken => 10,

        SyntaxKind.PlusToken or SyntaxKind.MinusToken => 20,

        SyntaxKind.StarToken or SyntaxKind.SlashToken => 30,


        _ => InvalidOperatorPrecedence
    };

    public static Optional<int> GetUnaryOperatorPrecedence(this SyntaxKind kind) => kind switch
    {
        SyntaxKind.StarToken or SyntaxKind.SlashToken => 10,
        SyntaxKind.PlusToken or SyntaxKind.MinusToken => 20,
        _ => default(Optional<int>) 
    };
}