using BenchmarkDotNet.Attributes;
using Bogus;
using Datatent3.Common;
using Datatent3.Common.Extensions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Core.Bench.Extensions
{
    [HtmlExporter, CsvExporter(), CsvMeasurementsExporter(),
        RankColumn(), KurtosisColumn, SkewnessColumn, StdDevColumn, MeanColumn, MedianColumn, MediumRunJob, MemoryDiagnoser]
    public class SpanExtensionBenchmark
    {
        private byte[] _demoData = null!;

        [GlobalSetup]
        public void Setup()
        {
            Randomizer randomizer = new Randomizer();
            _demoData = randomizer.Bytes(Constants.PageSize);
        }

        [Benchmark]
        public int ReadByteBench()
        {
            int i = 0;
            Span<byte> bytes = _demoData;

            for (int t = 0; t < Constants.PageSize - 1; t++)
            {
                i += bytes.ReadByte(t);
            }

            return i;
        }

        [Benchmark]
        public int ReadBytesBench()
        {
            int i = 0;
            Span<byte> bytes = _demoData;

            for (int t = 0; t < Constants.PageSize - 2; t++)
            {
                i += bytes.ReadBytes(t, Constants.PageSize - t).Length;
            }

            return i;
        }

        [Benchmark]
        public int ReadBytesSafeBench()
        {
            int i = 0;
            Span<byte> bytes = _demoData;

            for (int t = 0; t < Constants.PageSize - 2; t++)
            {
                i += bytes.ReadBytesSafe(t, Constants.PageSize - t).Length;
            }

            return i;
        }

        [Benchmark]
        public int ReadBytesRentedBench()
        {
            int i = 0;
            Span<byte> bytes = _demoData;

            for (int t = 0; t < Constants.PageSize - 2; t++)
            {
                var arr = bytes.ReadBytesRented(t, Constants.PageSize - t);

                i += arr.Length;
                ArrayPool<byte>.Shared.Return(arr);
            }

            return i;
        }
    }
}
