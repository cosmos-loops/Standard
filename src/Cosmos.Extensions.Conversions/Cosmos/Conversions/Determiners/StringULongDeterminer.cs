using System;
using System.Collections.Generic;
using System.Globalization;
using Cosmos.Conversions.Common;
using Cosmos.Conversions.Common.Core;
using Cosmos.Exceptions;

namespace Cosmos.Conversions.Determiners
{
    /// <summary>
    /// Internal core conversion helper from string to ulong
    /// </summary>
    public static class StringULongDeterminer
    {
        // ReSharper disable once InconsistentNaming
        internal static bool IS(string str) => Is(str);

        // ReSharper disable once InconsistentNaming
        private const NumberStyles NUMBER_STYLES = NumberStyles.AllowLeadingWhite
                                                 | NumberStyles.AllowTrailingWhite
                                                 | NumberStyles.AllowLeadingSign
                                                 | NumberStyles.AllowDecimalPoint
                                                 | NumberStyles.AllowThousands
                                                 | NumberStyles.AllowExponent;

        /// <summary>
        /// Is
        /// </summary>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="matchedCallback"></param>
        /// <returns></returns>
        public static bool Is(
            string text,
            NumberStyles style = NUMBER_STYLES,
            IFormatProvider formatProvider = null, 
            Action<ulong> matchedCallback = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
            
            return ulong.TryParse(text, style, formatProvider.SafeNumber(), out var number)
                        .IfFalseThenInvoke(ValueDeterminer.IsXxxAgain<ulong>, text)
                        .IfTrueThenInvoke(matchedCallback, number);
        }

        /// <summary>
        /// Is
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tries"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="matchedCallback"></param>
        /// <returns></returns>
        public static bool Is(
            string text, 
            IEnumerable<IConversionTry<string, ulong>> tries,
            NumberStyles style = NUMBER_STYLES, 
            IFormatProvider formatProvider = null, 
            Action<ulong> matchedCallback = null)
        {
            return ValueDeterminer.IsXXX(text, string.IsNullOrWhiteSpace,
                (s, act) => Is(s, style, formatProvider.SafeNumber(), act), tries, matchedCallback);
        }

        /// <summary>
        /// To
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultVal"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static ulong To(
            string text, 
            ulong defaultVal = default,
            NumberStyles style = NUMBER_STYLES,
            IFormatProvider formatProvider = null)
        {
            if (text is null)
                return defaultVal;

            if (ulong.TryParse(text, style, formatProvider.SafeNumber(), out var number))
                return number;

            return Try.Create(() => Convert.ToUInt64(Convert.ToDecimal(text)))
                      .Recover(_ => ValueConverter.ToXxxAgain(text, defaultVal))
                      .Value;
        }

        /// <summary>
        /// To
        /// </summary>
        /// <param name="text"></param>
        /// <param name="impls"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static ulong To(
            string text, 
            IEnumerable<IConversionImpl<string, ulong>> impls,
            NumberStyles style = NUMBER_STYLES,
            IFormatProvider formatProvider = null)
        {
            return ValueConverter.ToXxx(text, (s, act) => Is(s, style, formatProvider.SafeNumber(), act), impls);
        }
    }
}