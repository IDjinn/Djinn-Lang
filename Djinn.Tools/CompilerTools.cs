using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Djinn.Compile;
using Microsoft.CodeAnalysis;

namespace Djinn.Tools
{
    public static class CompilerTools
    {
        public static async Task<int> RunErrorLevelCommand(string outputFileName)
        {
            using var cmd = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = outputFileName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            cmd.Start();
            await cmd.WaitForExitAsync();
            return cmd.ExitCode;
        }

        public static async Task<string> RunCommandAsync(string[] args)
        {
            using var cmd = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = string.Join(" ", args),
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            cmd.Start();
            await cmd.StandardInput.WriteLineAsync(string.Join(" ", args));
            cmd.StandardInput.Close();

            var output = await cmd.StandardOutput.ReadToEndAsync();
            await cmd.WaitForExitAsync();

            return output;
        }

        public static async Task<string> RunClangAsync(string[] args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "clang",
                Arguments = string.Join(" ", args),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                var output = new StringBuilder();

                process.OutputDataReceived += (sender, e) => output.AppendLine(e.Data);
                process.ErrorDataReceived += (sender, e) => output.AppendLine(e.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await process.WaitForExitAsync();

                return output.ToString();
            }
        }

        public static Compiler.CompilationResult GenerateIR(string source, Compiler.CompilerOptions? options = null)
        {
            return Compiler.Compile(source, options ?? new Compiler.CompilerOptions("test", "test"));
        }

        public static async Task<string> ClangCompileAsync(Compiler.CompilationResult compilationResult,
            Compiler.CompilerOptions options)
        {
            Compiler.WriteToFile(compilationResult, options);
            return await RunClangAsync([$"{options.OutputFileName}.ll", "-o", $"{options.OutputFileName}.exe"]);
        }

        public static async Task<string> RunAsync(string exeFile)
        {
            return await RunCommandAsync([$"{exeFile}.exe"]);
        }

        public readonly record struct CompileAndRunResult(
            string IR,
            int ErrorLevel
        );
        public static async Task<CompileAndRunResult> CompileAndRun(string source, [CallerMemberName] string caller = null)
        {
            var compilerOptions = new Compiler.CompilerOptions($"./temp/{caller}", caller);
            var ir = GenerateIR(source, compilerOptions);
            await ClangCompileAsync(ir, compilerOptions);
            await RunAsync(compilerOptions.OutputFileName);
            return new(ir.Ir,await RunErrorLevelCommand(compilerOptions.OutputFileName));
        }
    }

    public static class ProcessExtensions
    {
        public static Task<int> WaitForExitAsync(this Process process)
        {
            var tcs = new TaskCompletionSource<int>();
            process.Exited += (_, __) => tcs.TrySetResult(process.ExitCode);
            if (process.HasExited)
                tcs.TrySetResult(process.ExitCode);
            return tcs.Task;
        }
    }
}