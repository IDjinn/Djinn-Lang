﻿using System.Runtime.InteropServices;
using Djinn.Compile;
using Djinn.Lexing;
using Djinn.Parsing;
using Djinn.Syntax.Biding;
using LLVMSharp;

namespace Djinn;

public static class Program
{
    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int printf(string format, params object[] args);


    public static void Main(string[] args)
    {
// var source = $$"""
//                function void hello(void) {
//                    ret printf("Hello World!");
//                }
//                """;
//
//         var source = $$"""
//                        function void hello(int a) {
//                            ret 1 + 2;
//                        }
//                        """;

//                            string someString = "";
        // var source = $$"""1+2+3+4""";

        var source = $$"""
                       function int32 sum(int32 a, int32 b) {
                            ret a + b;
                       }
                       
                       function int32 main() {
                        ret sum(1, 1);
                       }
                       """;


        var lexer = new Lexer(source);
        var parser = new Parser(lexer);
        var tree = parser.Parse();

        var binder = new Binder();
        var bindResult = binder.Bind(tree);
        Compiler.Compile(bindResult);
        return;
        
        var compiler = new LLVMCompiler(bindResult);
        compiler.Compile();

        string moduleError = "";
        LLVM.VerifyModule(compiler.Module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out moduleError);
        Console.WriteLine(moduleError);

        string error = "";
        LLVM.DumpModule(compiler.Module);
        LLVM.PrintModuleToFile(compiler.Module, "test.ll", out error);
        Console.WriteLine(error);
        return;
    }
}