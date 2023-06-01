using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public class NativeMemorySlabPool : MemorySlabPoolBase
    {
        /// <summary>
        /// The buffer pool
        /// </summary>
        private static readonly Lazy<NativeMemorySlabPool>
            LAZY =
                new Lazy<NativeMemorySlabPool>
                    (() =>
                    {
                        //if (InitFunction == null)
                        //{
                        //    throw new Exception($"{nameof(InitFunction)} can't be null!");
                        //}

                        //var vals = InitFunction();
                        return new NativeMemorySlabPool();
                    });

        /// <summary>
        /// The shared instance
        /// </summary>
        public new static NativeMemorySlabPool Shared => LAZY.Value;

        ///// <summary>
        ///// The init function to setup the pool
        ///// </summary>
        //public static Func<(DatatentSettings?, ILogger)> InitFunction { get; set; } = () => new(null, NullLogger.Instance);

        private ILogger _logger = NullLogger.Instance;
        private readonly IntPtr _memoryPtr;
        private readonly Memory<byte> _memory;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly UnmanagedMemoryManager<byte> _memoryManager;
        /// <summary>
        /// The list of free slots available for renting
        /// </summary>
        private readonly Queue<int> _freeSlots = new Queue<int>(5000);

        /// <summary>
        /// The logger instance
        /// </summary>
        public ILogger Logger
        {
            set => _logger = value;
        }

        public int FreeSlots => _freeSlots.Count;

        /// <summary>
        /// Gets the pointer to a slot in the unmanaged memory.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An IntPtr.</returns>
        public IntPtr GetPointerToSlot(int key)
        {
            return IntPtr.Add(_memoryPtr, Constants.PageSize * (key - 1));
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="datatentSettings"></param>
        /// <param name="logger"></param>
        public unsafe NativeMemorySlabPool()
        {
            // alloc the memory
            // TODO: in .net 6 replace with NativeMemory.Alloc            
            _memoryPtr = (nint) NativeMemory.AllocZeroed((nuint)MaxBufferSize);
            _memoryManager = new UnmanagedMemoryManager<byte>((byte*)_memoryPtr, MaxBufferSize);
            _memory = _memoryManager.Memory;
            // save all available page buffers for renting
            foreach (var i in Enumerable.Range(1, MaxBufferSize / Constants.PageSize))
            {
                _freeSlots.Enqueue(i);
            }
        }

        /// <summary>
        /// Free slots available
        /// </summary>
        /// <returns></returns>
        public bool HasFreeSlots()
        {
            return _freeSlots.Count > 0;
        }

        /// <summary>
        /// Return a buffer to the pool
        /// </summary>
        /// <param name="segment"></param>
        public override void Return(IMemorySlab segment)
        {
            segment.Clear();
            _freeSlots.Enqueue(((NativeMemorySlab)segment).Key);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            _freeSlots.Clear();
            // free the unmanaged memory
            Marshal.FreeHGlobal(_memoryPtr);
        }

        /// <inheritdoc />
        public override IMemorySlab Rent(int minBufferSize = -1)
        {
            var freeKey = _freeSlots.Dequeue();
            return new NativeMemorySlab(_memory.Slice(Constants.PageSize * (freeKey - 1), Constants.PageSize),
                freeKey, this);
        }

        /// <summary>
        /// Rent
        /// </summary>
        /// <returns></returns>
        private IMemorySlab RentCore() => this.Rent();

        /// <summary>
        /// The maximum size of the buffer pool for renting
        /// </summary>
        public sealed override int MaxBufferSize =>
            Constants.PageSize * 5000;
    }
}
