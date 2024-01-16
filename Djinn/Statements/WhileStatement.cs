﻿using Djinn.Expressions;
using Djinn.Syntax;
using Djinn.Syntax.Biding.Scopes;

namespace Djinn.Statements;

public record WhileStatement( 
    IExpressionSyntax Expression,
    BlockStatement Block
    ) : IStatement
{
    public SyntaxKind Kind => SyntaxKind.While;
    public T Visit<T>(IStatementVisitor<T> visitor, BoundScope boundScope)
    {
        return visitor.Visit(this, boundScope);
    }
}