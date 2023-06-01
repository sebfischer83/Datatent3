using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Memory
{
    public static class MemorySlabFactory
    {
        private static Lazy<MemorySlabPoolBase> _pool = new Lazy<MemorySlabPoolBase>(() => new ManagedMemorySlabPool());

        public static MemorySlabPoolBase GetImplementation()
        {
            return _pool.Value;
        }
    }
}
