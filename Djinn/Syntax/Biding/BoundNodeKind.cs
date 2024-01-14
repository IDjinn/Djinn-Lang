namespace Djinn.Syntax.Biding;

public enum BoundNodeKind
{
    Unknown,

    UnaryExpression,
    BinaryExpression,
    LiteralExpression,
    ConstantExpression,

    BlockStatement,
    ReturnStatement,
    FunctionStatement,
    FunctionIdentifier,
    FunctionParameter,
    ReadVariable,
    FunctionCall,
    Discard,
    If,
    Import,
    Switch,
    Case,
    DeclareVariable
}

public static class BoundNodeKindExtensions
{
    public static bool IsExpression(this BoundNodeKind kind) =>
        kind switch
        {
            BoundNodeKind.UnaryExpression
                or BoundNodeKind.LiteralExpression
                or BoundNodeKind.BinaryExpression
                or BoundNodeKind.LiteralExpression
                => true,
            _ => false
        };
}