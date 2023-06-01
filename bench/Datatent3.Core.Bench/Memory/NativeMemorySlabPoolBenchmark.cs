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
    public class NativeMemorySlabPoolBenchmark
    {
        private NativeMemorySlabPool _pool = null!;

        [GlobalSetup]
        public void Setup()
        {
            _pool = NativeMemorySlabPool.Shared;
        }

        [Benchmark]
        public int Benchmark()
        {
            int t = 0;
            for (int i = 0; i < 1000; i++)
            {
                var slab = _pool.Rent();
                t += (int) slab.Length;
                _pool.Return(slab);
            }

            return t;
        }
    }
}
