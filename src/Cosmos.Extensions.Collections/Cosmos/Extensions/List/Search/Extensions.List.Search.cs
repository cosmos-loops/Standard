using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Cosmos
{
    /// <summary>
    /// Extensions of list
    /// </summary>
    public static partial class ListExtensions
    {
        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<T>(this IList<T> list, T value)
        {
            return list.BinarySearch(t => t, value);
        }

        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="map"></param>
        /// <param name="value"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, Func<TSource, TValue> map, TValue value)
        {
            return list.BinarySearch(map, value, Comparer<TValue>.Default);
        }

        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<T>(this IList<T> list, int index, int length, T value)
        {
            return list.BinarySearch(index, length, t => t, value);
        }

        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="map"></param>
        /// <param name="value"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, int index, int length, Func<TSource, TValue> map, TValue value)
        {
            return list.BinarySearch(index, length, map, value, Comparer<TValue>.Default);
        }

        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<T>(this IList<T> list, T value, IComparer<T> comparer)
        {
            return list.BinarySearch(0, list.Count, t => t, value, comparer);
        }

        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="map"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, Func<TSource, TValue> map, TValue value, IComparer<TValue> comparer)
        {
            return list.BinarySearch(0, list.Count, map, value, comparer);
        }


        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int BinarySearch<T>(this IList<T> list, int index, int length, T value, IComparer<T> comparer)
        {
            return list.BinarySearch(index, length, t => t, value, comparer);
        }

        /// <summary>
        /// Binary search
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="map"></param>
        /// <param name="value"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static int BinarySearch<TSource, TValue>(this IList<TSource> list, int index, int length, Func<TSource, TValue> map, TValue value, IComparer<TValue> comparer)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"The {nameof(index)} parameter must be a non-negative value.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, $"The {nameof(length)} parmeter must be a non-negative value.");
            if (index + length > list.Count)
                throw new InvalidOperationException(
                    $"The value of {nameof(index)} plus {nameof(length)} must be less than or equal to the value of the number of items in the {nameof(list)}.");

            // Do work.
            // Taken from https://github.com/dotnet/coreclr/blob/cdff8b0babe5d82737058ccdae8b14d8ae90160d/src/mscorlib/src/System/Collections/Generic/ArraySortHelper.cs#L156
            // The lo and high bounds.
            var low = index;
            var high = index + length - 1;

            // While low < high.
            while (low <= high)
            {
                // The midpoint.
                var midpoint = low + ((high - low) >> 1);

                // Compare.
                var order = comparer.Compare(map(list[midpoint]), value);

                // If they are equal, return.
                if (order == 0) return midpoint;

                // If less than zero, reset low, otherwise, reset high.
                if (order < 0)
                    low = midpoint + 1;
                else
                    high = midpoint - 1;
            }

            // Nothing matched, return inverse of the low bound.
            return ~low;
        }
    }
}