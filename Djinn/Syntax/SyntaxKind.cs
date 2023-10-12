namespace Djinn.Syntax;

[Flags]
public enum SyntaxKind : long
{
    None = 0,
    Trivia = 1 << 2,
    LogicalOperators = 1 << 4,
    ArithmeticOperators = 1 << 5,
    Assignment = 1 << 6,
    Declaration = 1 << 8,
    Expression = 1 << 10,
    Identifier = 1 << 12,
    Variable = 1 << 16,
    Function = 1 << 18,
    Block = 1 << 20,

    ValueTypes = 1 << 22,

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

    BinaryExpression = Expression | 1,
    UnaryExpression = Expression | 2,
    VariableAssignmentExpression = Variable | LogicalOperators | Expression | 1,
    FunctionCallExpression = Function | Expression | 1,

    VariableDeclaration = Variable | Declaration | 1,
    FunctionDeclaration = Function | Declaration | 2,

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

    Void = ValueTypes | 1,

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
}