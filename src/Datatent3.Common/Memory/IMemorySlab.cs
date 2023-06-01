using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public interface IMemorySlab : IMemoryOwner<byte>
    {
        /// <summary>
        /// Clears the buffer
        /// </summary>
        public void Clear();

        /// <summary>
        /// The length of the buffer
        /// </summary>
        public uint Length { get; }

        /// <summary>
        /// The buffer as a span
        /// </summary>
        public Span<byte> Span { get; }
    }
}
