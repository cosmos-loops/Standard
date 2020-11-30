﻿using System;

namespace Cosmos.Conversions
{
    /// <summary>
    /// Binary Conversion Utilities
    /// </summary>
    public static class BinaryConv
    {
        /// <summary>
        /// Convert from binary to decimalism.
        /// </summary>
        /// <example>in: 101110; out: 46</example>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static int ToDecimalism(string bin) => Convert.ToInt32(bin, 2);

        /// <summary>
        /// Convert from binary to hexadecimal.
        /// </summary>
        /// <example>in: 101110; out: 2E</example>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string ToHexadecimal(string bin) => DecimalismConv.ToHexadecimal(ToDecimalism(bin));
    }
}