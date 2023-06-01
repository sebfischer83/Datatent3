using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public class ManagedMemorySlab : IMemorySlab
    {
        public int Key { get; }
        private readonly ManagedMemorySlabPool _pool;
        private bool _disposed;
        public Memory<byte> Memory { get; }

        public ManagedMemorySlab(Memory<byte> memory, int key, ManagedMemorySlabPool pool)
        {
            Memory = memory;
            Key = key;
            _pool = pool;
        }

        public uint Length => (uint)Memory.Length;

        public Span<byte> Span => Memory.Span;

        public void Clear()
        {
            Memory.Span.Clear();
        }

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(NativeMemorySlab));

            _pool.Return(this);
            _disposed = true;
        }
    }
}
