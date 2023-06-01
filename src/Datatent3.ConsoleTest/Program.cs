using Bogus;
using Datatent3.Common;
using Datatent3.Common.Extensions;
using System.Buffers;

Randomizer randomizer = new Randomizer();
var demoData = randomizer.Bytes(Constants.PageSize * 1000);
int i = 0;
Span<byte> bytes = demoData;

for (int h = 0; h < 5000; h++)
{
    for (int t = 0; t < 1000 - 1; t++)
    {
        var arr = bytes.ReadBytesToSlab(t * Constants.PageSize, Constants.PageSize);

        i += arr.Span.Length;
        arr.Dispose();
    }
}
for (int h = 0; h < 5000; h++)
{
    for (int t = 0; t < 1000 - 1; t++)
    {
        var arr = bytes.ReadBytesRented(t * Constants.PageSize, Constants.PageSize);

        i += arr.Length;
        ArrayPool<byte>.Shared.Return(arr, true);
    }
}
