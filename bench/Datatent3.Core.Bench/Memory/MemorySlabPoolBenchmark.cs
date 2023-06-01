using BenchmarkDotNet.Attributes;
using Bogus;
using Datatent3.Common.Memory;
using Iced.Intel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Bench.Memory
{
    [HtmlExporter, CsvExporter(), CsvMeasurementsExporter(),
    RankColumn(), KurtosisColumn, SkewnessColumn, StdDevColumn, MeanColumn, MedianColumn, MediumRunJob, MemoryDiagnoser]
    public class MemorySlabPoolBenchmark
    {
        private NativeMemorySlabPool _nativePool = null!;
        private ManagedMemorySlabPool _managedPool = null!;

        [GlobalSetup]
        public void Setup()
        {
            _nativePool = NativeMemorySlabPool.Shared;
            _managedPool = ManagedMemorySlabPool.Shared;
        }

        [Benchmark]
        public int NativeBenchmark()
        {
            int t = 0;
            for (int i = 0; i < 1000; i++)
            {
                var slab = _nativePool.Rent();
                t += (int) slab.Length;
                _nativePool.Return(slab);
            }

            return t;
        }

        [Benchmark]
        public int ManagedBenchmark()
        {
            int t = 0;
            for (int i = 0; i < 1000; i++)
            {
                var slab = _managedPool.Rent();
                t += (int)slab.Length;
                _managedPool.Return(slab);
            }

            return t;
        }
    }
}
