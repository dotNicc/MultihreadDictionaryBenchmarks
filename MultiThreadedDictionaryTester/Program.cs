using BenchmarkDotNet.Running;
using Serilog;

namespace MultiThreadedDictionaryTester
{
    static class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            
            Log.Information("Starting benchmarks");
            BenchmarkRunner.Run<DictionaryBenchmarks>();
        }
    }
}