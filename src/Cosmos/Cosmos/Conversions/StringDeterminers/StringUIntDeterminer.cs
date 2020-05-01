using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cosmos.Conversions.StringDeterminers {
    /// <summary>
    /// Internal core conversion helper from string to uint
    /// </summary>
    public static class StringUIntDeterminer {
        /// <summary>
        /// Is
        /// </summary>
        /// <param name="str"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="intAct"></param>
        /// <returns></returns>
        public static bool Is(string str, NumberStyles style = NumberStyles.Integer,
            IFormatProvider formatProvider = null, Action<uint> intAct = null) {

            if (string.IsNullOrWhiteSpace(str))
                return false;

            var result = uint.TryParse(str, style, formatProvider.SafeN(), out var number);

            if (result) {
                intAct?.Invoke(number);
            }

            return result;
        }

        /// <summary>
        /// Is
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tries"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <param name="intAct"></param>
        /// <returns></returns>
        public static bool Is(string str, IEnumerable<IConversionTry<string, uint>> tries,
            NumberStyles style = NumberStyles.Integer, IFormatProvider formatProvider = null, Action<uint> intAct = null) {
            return _Helper.IsXXX(str, string.IsNullOrWhiteSpace,
                (s, act) => Is(s, style, formatProvider.SafeN(), act), tries, intAct);
        }

        /// <summary>
        /// To
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultVal"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static uint To(string str, uint defaultVal = default,
            NumberStyles style = NumberStyles.Integer, IFormatProvider formatProvider = null) =>
            uint.TryParse(str, style, formatProvider.SafeN(), out var number) ? number : defaultVal;

        /// <summary>
        /// To
        /// </summary>
        /// <param name="str"></param>
        /// <param name="impls"></param>
        /// <param name="style"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static uint To(string str, IEnumerable<IConversionImpl<string, uint>> impls,
            NumberStyles style = NumberStyles.Integer, IFormatProvider formatProvider = null) =>
            _Helper.ToXXX(str, (s, act) => Is(s, style, formatProvider.SafeN(), act), impls);
    }
}