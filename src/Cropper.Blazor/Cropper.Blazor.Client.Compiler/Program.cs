using System;
using System.Diagnostics;

namespace Cropper.Blazor.Client.Compiler
{
    public class Program
    {
        public static int Main()
        {
            var stopWatch = Stopwatch.StartNew();
            var success =
                new CodeSnippetsCompiler().Execute()
                && new DocStrings().Execute();

            Console.WriteLine($"Docs.Compiler completed in {stopWatch.ElapsedMilliseconds} msecs");

            return success ? 0 : 1;
        }
    }
}
