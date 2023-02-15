using Ao.Stock.Benchmarks.Runners;
using BenchmarkDotNet.Running;

namespace Ao.Stock.Benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var o = new ORMBenchmarks();
            //o.Init();
            //o.ORM().GetAwaiter().GetResult();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
        }
    }
}