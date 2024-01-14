using System.Runtime.CompilerServices;

namespace Djinn.Tests;

public static class Setup
{
    [ModuleInitializer]
    public static void InitializeVerify()
    {
        if (!Directory.Exists("./temp"))
            Directory.CreateDirectory("./temp");
    }
}