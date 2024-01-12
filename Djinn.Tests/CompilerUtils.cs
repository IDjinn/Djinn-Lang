using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Djinn.Tests
{
    public static class CompilerUtils
    {
        public static async Task<string> RunCommandAsync(string[] args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var cmd = new Process { StartInfo = startInfo })
            {
                //cmd.OutputDataReceived += (sender, e) => output.Append(e.Data);
                cmd.Start();
             
                await cmd.StandardInput.WriteLineAsync(string.Join(" ", args));
                cmd.StandardInput.Close();  // Close the input stream to signal that no more input will be provided

                //cmd.BeginOutputReadLine();

               var output = await cmd.StandardOutput.ReadToEndAsync();
                await cmd.WaitForExitAsync();  // Wait for the process to exit

                return output;  // You may return some meaningful result based on your requirements
            }
        }


        public static async Task CompileAsync(string source)
        {
            var llvmResult = await RunCommandAsync(new[] { "clang", "test.ll", "-o", "test.exe" });

            Console.WriteLine(llvmResult);
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