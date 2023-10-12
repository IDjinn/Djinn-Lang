using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Djinn;

public class IDjinn
{
    private readonly ILogger<IDjinn> _logger = new Logger<IDjinn>(NullLoggerFactory.Instance);

    public static void Compile(string source)
    {
        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();
        var binder = new Binder();

        var any = tree.Generate(binder);

        // var codeGen = new CodeGen(tree);
        // var module = codeGen.GenerateLlvm();
        //
        // string moduleError = "";
        // LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out moduleError);
        // Console.WriteLine(moduleError);
        //
        // string error = "";
        // LLVM.DumpModule(module);
        // LLVM.PrintModuleToFile(module, "test.ll", out error);
        // Console.WriteLine(error);
    }
}