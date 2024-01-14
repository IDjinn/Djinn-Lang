using Djinn.Compile;
using LLVMSharp;

namespace Djinn.Syntax.Biding.Scopes.Types;

public readonly record struct BoundType(
    string Identifier,
    IType Type
)
{
}