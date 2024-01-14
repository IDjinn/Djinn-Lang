using Djinn.Compile;
using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Statements;
using Djinn.Syntax.Biding;
using Microsoft.CodeAnalysis.CSharp;

namespace Djinn.Tests;

public class TestsUtilities
{
    public const string BasePath = "./temp";
    public static Compiler.CompilerOptions GenerateForTest(Type clazz)
    {
        var cleanName = string.Join("", clazz.Name.Where(ch => char.IsAsciiLetterOrDigit(ch) || ch == '_' || ch == '-'));
        return new Compiler.CompilerOptions($"{BasePath}/{cleanName}");
    }
}