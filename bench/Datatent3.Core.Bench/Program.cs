using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Datatent3.Common.Bench
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}