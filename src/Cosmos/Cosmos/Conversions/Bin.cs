﻿using Cosmos.Conversions.Helpers;

namespace Cosmos.Conversions
{
    public static class Bin
    {
        /// <summary>
        /// Reverse high and low positions.
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static string Reverse(string bin)
        {
            return ScaleRevHelper.Reverse(bin, 8);
        }
    }
}