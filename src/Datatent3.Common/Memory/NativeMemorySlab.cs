using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public class NativeMemorySlab : IMemorySlab
    {
        /// <summary>
        /// The key represents the position in the unmanaged memory block
        /// </summary>
        public int Key { get; }
        private readonly NativeMemorySlabPool _pool;
        private bool _disposed;

        /// <summary>
        /// Gets a raw pointer to the pool for this segment
        /// </summary>
        /// <returns></returns>
        public unsafe byte* GetPointer()
        {
            return (byte*)_pool.GetPointerToSlot(Key);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="memory"></param>
        /// <param name="key"></param>
        /// <param name="pool"></param>
        public NativeMemorySlab(Memory<byte> memory, int key, NativeMemorySlabPool pool)
        {
            Memory = memory;
            Key = key;
            _pool = pool;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(NativeMemorySlab));

            _pool.Return(this);
            _disposed = true;
        }

        /// <inheritdoc />
        public Memory<byte> Memory { get; }

        /// <inheritdoc />
        public void Clear()
        {
            Memory.Span.Clear();
        }

        /// <inheritdoc />
        public uint Length => (uint)Memory.Length;

        /// <inheritdoc />
        public Span<byte> Span => Memory.Span;
    }
}
