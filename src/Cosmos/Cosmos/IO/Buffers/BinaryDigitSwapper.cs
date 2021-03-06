﻿namespace Cosmos.IO.Buffers
{
    /// <summary>
    /// Bit Swapper
    /// </summary>
    public static class BinaryDigitSwapper
    {
        public static short SwapInt16(short v) =>
            (short) (((v & 0xff) << 8) | ((v >> 8) & 0xff));

        public static ushort SwapUInt16(ushort v) =>
            (ushort) (((v & 0xff) << 8) | ((v >> 8) & 0xff));

        public static int SwapInt32(int v) =>
            ((SwapInt16((short) v) & 0xffff) << 0x10) | (SwapInt16((short) (v >> 0x10)) & 0xffff);

        public static uint SwapUInt32(uint v) =>
            (uint) (((SwapUInt16((ushort) v) & 0xffff) << 0x10) | (SwapUInt16((ushort) (v >> 0x10)) & 0xffff));

        public static long SwapInt64(long v) =>
            ((SwapInt32((int) v) & 0xffffffffL) << 0x20) | (SwapInt32((int) (v >> 0x20)) & 0xffffffffL);

        public static ulong SwapUInt64(ulong v) =>
            (ulong) (((SwapUInt32((uint) v) & 0xffffffffL) << 0x20) | (SwapUInt32((uint) (v >> 0x20)) & 0xffffffffL));
    }
}