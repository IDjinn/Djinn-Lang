using Djinn.Compile.Scopes;
using Djinn.Syntax;

namespace Djinn.Compile.Variables;

public interface IVariable
{
    public string Identifier { get; }
    public IScope Scope { get; }
    public SyntaxKind Kind { get; }
    public dynamic ConstantValue { get; }
}