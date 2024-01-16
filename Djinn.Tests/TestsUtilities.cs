using Djinn.Compile;

namespace Djinn.Tests;

public class TestsUtilities
{
    public const string BasePath = "./temp";

    public static Compiler.CompilerOptions GenerateForTest(Type clazz)
    {
        var cleanName = string.Join("",
            clazz.Name.Where(ch => char.IsAsciiLetterOrDigit(ch) || ch == '_' || ch == '-'));
        return new Compiler.CompilerOptions($"{BasePath}/{cleanName}", cleanName);
    }
}