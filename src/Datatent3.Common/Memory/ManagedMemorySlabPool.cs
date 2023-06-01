using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public class ManagedMemorySlabPool : MemorySlabPoolBase
    {
        private static readonly Lazy<ManagedMemorySlabPool>
            LAZY =
                new Lazy<ManagedMemorySlabPool>
                    (() =>
                    {
                        //if (InitFunction == null)
                        //{
                        //    throw new Exception($"{nameof(InitFunction)} can't be null!");
                        //}

                        //var vals = InitFunction();
                        return new ManagedMemorySlabPool();
                    });

        private Memory<byte> _buffer;

        public new static ManagedMemorySlabPool Shared => LAZY.Value;

        public override int MaxBufferSize => Constants.PageSize * 5000;

        private readonly Queue<int> _freeSlots = new Queue<int>(5000);

        public ManagedMemorySlabPool()
        {
            _buffer = new Memory<byte>(new byte[MaxBufferSize]);
            // save all available page buffers for renting
            foreach (var i in Enumerable.Range(1, MaxBufferSize / Constants.PageSize))
            {
                _freeSlots.Enqueue(i);
            }
        }

        public override IMemorySlab Rent(int minBufferSize = -1)
        {
            var freeKey = _freeSlots.Dequeue();
            return new ManagedMemorySlab(_buffer.Slice(Constants.PageSize * (freeKey - 1), Constants.PageSize),
                freeKey, this);
        }

        public override void Return(IMemorySlab segment)
        {
            segment.Clear();
            _freeSlots.Enqueue(((ManagedMemorySlab)segment).Key);
        }

        protected override void Dispose(bool disposing)
        {
            _freeSlots.Clear();
        }
    }
}
