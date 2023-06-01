using Datatent3.Common.Memory;

namespace Datatent3.Common.Tests
{
    public class NativeMemorySlapPoolTests
    {
        [Fact]
        public void Create_Slab_Test()
        {
            NativeMemorySlabPool pool = NativeMemorySlabPool.Shared;

            var initialSize = pool.FreeSlots;
            var slab = pool.Rent();
            slab.Should().NotBeNull();
            slab.Length.Should().Be(Constants.PageSize);
            pool.FreeSlots.Should().Be(initialSize - 1);

            pool.Return(slab);
            pool.FreeSlots.Should().Be(initialSize);
        }
    }
}