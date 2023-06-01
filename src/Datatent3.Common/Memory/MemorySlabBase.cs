using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public abstract class MemorySlabPoolBase : MemoryPool<byte>
    {
        /// <summary>
        /// Return a buffer to the pool
        /// </summary>
        /// <param name="segment"></param>
        public abstract void Return(IMemorySlab segment);

        /// <summary>
        /// Rent a buffer from  the pool
        /// </summary>
        /// <param name="minBufferSize"></param>
        /// <returns></returns>
        public abstract override IMemorySlab Rent(int minBufferSize = -1);
    }
}
