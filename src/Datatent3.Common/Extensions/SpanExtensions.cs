﻿using Datatent3.Common.Memory;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Datatent3.Common.Extensions
{
    public static class SpanExtensions
    {
        /// <summary>
        /// Reads a byte.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static byte ReadByte(this Span<byte> span, int offset)
        {
            return span[offset];
        }

        /// <summary>
        /// Reads bytes.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe byte[] ReadBytes(this Span<byte> span, int offset, int length)
        {
            var returnArray = new byte[length];

            fixed (byte* bp = span.Slice(offset))
            fixed (byte* rp = returnArray)
            {
                Unsafe.CopyBlock(rp, bp, (uint)length);
            }

            return returnArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe IMemorySlab ReadBytesToSlab(this Span<byte> span, int offset, int length)
        {
            var returnArray = MemorySlabFactory.GetImplementation().Rent();

            fixed (byte* bp = span.Slice(offset))
            fixed (byte* rp = returnArray.Span)
            {
                Unsafe.CopyBlock(rp, bp, (uint)length);
            }

            return returnArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe byte[] ReadBytesRented(this Span<byte> span, int offset, int length)
        {
            var returnArray = ArrayPool<byte>.Shared.Rent(length);

            fixed (byte* bp = span.Slice(offset))
            fixed (byte* rp = returnArray)
            {
                Unsafe.CopyBlock(rp, bp, (uint)length);
            }

            return returnArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe byte[] ReadBytesSafe(this Span<byte> span, int offset, int length)
        {
            var returnArray = new byte[length];

            span.Slice(offset, length).CopyTo(returnArray);
            
            return returnArray;
        }

        /// <summary>
        /// Reads a bool.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool ReadBool(this Span<byte> span, int offset)
        {
            return span[offset] != 0;
        }

        /// <summary>
        /// Reads the a uint32.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static uint ReadUInt32(this Span<byte> span, int offset)
        {
            return BitConverter.ToUInt32(span.Slice(offset));
        }

        /// <summary>
        /// Reads a uint16.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ushort ReadUInt16(this Span<byte> span, int offset)
        {
            return BitConverter.ToUInt16(span.Slice(offset));
        }

        /// <summary>
        /// Reads a unique identifier.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Guid ReadGuid(this Span<byte> span, int offset)
        {
            var guidSpan = span.Slice(offset, 16);
            return MemoryMarshal.Read<Guid>(guidSpan);
        }

        /// <summary>
        /// Write bytes.
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="bytes">The bytes.</param>
        /// <exception cref="System.ArgumentNullException">bytes</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void WriteBytes(this Memory<byte> memory, int offset, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            fixed (byte* bp = bytes)
            fixed (byte* rp = memory.Span.Slice(offset))
            {
                Unsafe.CopyBlock(rp, bp, (uint)bytes.Length);
            }

        }

        /// <summary>
        /// Write bytes.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="bytes">The bytes.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void WriteBytes(this ref Span<byte> span, int offset, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            fixed (byte* bp = bytes)
            fixed (byte* rp = span.Slice(offset))
            {
                Unsafe.CopyBlock(rp, bp, (uint)bytes.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void WriteBytes(this ref Span<byte> span, int offset, Span<byte> bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            fixed (byte* bp = bytes)
            fixed (byte* rp = span.Slice(offset))
            {
                Unsafe.CopyBlock(rp, bp, (uint)bytes.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteBytesSafe(this Memory<byte> memory, int offset, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            bytes.CopyTo(memory.Span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteBytesSafe(this ref Span<byte> span, int offset, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            bytes.CopyTo(span.Slice(offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void WriteBytesSafe(this ref Span<byte> span, int offset, Span<byte> bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            bytes.CopyTo(span.Slice(offset));
        }

        /// <summary>
        /// Writes the values to the span.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="values">The values.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void Write<T>(this Span<T> span, int offset, Span<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            int i = offset;
            foreach (var b in values)
            {
                span[i] = b;
                i++;
            }
        }

        /// <summary>
        /// Writes a byte.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="b">The b.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteByte(this Span<byte> span, int offset, byte b)
        {
            span[offset] = b;
        }

        /// <summary>
        /// Writes a bool.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="b">if set to <c>true</c> [b].</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteBool(this Span<byte> span, int offset, bool b)
        {
            span[offset] = (byte)(b ? 1 : 0);
        }

        /// <summary>
        /// Writes a unique identifier.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="id">The identifier.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteGuid(this Span<byte> span, int offset, Guid id)
        {
            span.WriteBytes(offset, id.ToByteArray());
        }

        /// <summary>
        /// Writes a uint32.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="val">The value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteUInt32(this Span<byte> span, int offset, uint val)
        {
            var b = BitConverter.GetBytes(val);
            span.WriteBytes(offset, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteUInt64(this Span<byte> span, int offset, ulong val)
        {
            var b = BitConverter.GetBytes(val);
            span.WriteBytes(offset, b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void WriteInt64(this Span<byte> span, int offset, long val)
        {
            var b = BitConverter.GetBytes(val);
            span.WriteBytes(offset, b);
        }
    }
}
