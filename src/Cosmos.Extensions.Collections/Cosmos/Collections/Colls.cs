using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable MemberHidesStaticFromOuterClass

namespace Cosmos.Collections
{
    internal static class LinqCollsHelper
    {
        public static IEnumerable<T> EnumerableTakeLast<T>(IEnumerable<T> source, int count)
        {
            var window = new Queue<T>(count);

            foreach (T item in source)
            {
                window.Enqueue(item);
                if (window.Count > count)
                    window.Dequeue();
            }

            return window;
        }

        public static IEnumerable<T> CollectionTakeLast<T>(ICollection<T> source, int count)
        {
            count = Math.Min(source.Count, count);

            if (count == 0)
                return Enumerable.Empty<T>();

            if (count == source.Count)
                return source;

            return source.Skip(source.Count - count);
        }

        public static IEnumerable<T> ReadOnlyCollectionTakeLast<T>(IReadOnlyCollection<T> source, int count)
        {
            count = Math.Min(source.Count, count);

            if (count == 0)
                return Enumerable.Empty<T>();

            if (count == source.Count)
                return source;

            return source.Skip(source.Count - count);
        }
    }

    /// <summary>
    /// Collections utilities
    /// </summary>
    public static partial class Colls
    {
        #region Chunk

        /// <summary>
        /// Chunk
        /// </summary>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int size)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), size, $"The {nameof(size)} parameter must be a positive value.");

            using (var enumerator = source.GetEnumerator())
            {
                do
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    yield return ChunkSequence(enumerator, size);
                } while (true);
            }
        }

        private static IEnumerable<T> ChunkSequence<T>(IEnumerator<T> enumerator, int size)
        {
            if (enumerator is null)
                throw new ArgumentNullException(nameof(enumerator));

            var count = 0;

            do
            {
                yield return enumerator.Current;
            } while (++count < size && enumerator.MoveNext());
        }

        #endregion

        #region BeContainedIn

        /// <summary>
        /// In
        /// </summary>
        /// <param name="item"></param>
        /// <param name="items"></param>
        /// <param name="equalityComparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool BeContainedIn<T>(T item, IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));
            if (equalityComparer is null)
                throw new ArgumentNullException(nameof(equalityComparer));

            return items.Contains(item, equalityComparer);
        }

        #endregion
        
        #region Contains

        /// <summary>
        /// Check whether the set contains members of the given condition.<br />
        /// 检查集合中是否包含给定条件的成员。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Contains<T>(IEnumerable<T> source, Expression<Func<T, bool>> condition)
        {
            if (condition is null)
                throw new ArgumentNullException(nameof(condition));
            var func = condition.Compile();
            return source.Any(item => func.Invoke(item));
        }

        /// <summary>
        /// Check if a set has a specified number of members. <br />
        /// 检查一个集合是否拥有指定数量的成员
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ContainsAtLeast<T>(ICollection<T> source, int count)
        {
            return source?.Count >= count;
        }

        /// <summary>
        /// Check if a set has a specified number of members. <br />
        /// 检查一个集合是否拥有指定数量的成员
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ContainsAtLeast<T>(IQueryable<T> source, int count)
        {
            if (source is null)
                return false;
            return source.Take(count).Count() >= count;
        }

        #endregion

        #region Empty

        /// <summary>
        /// Create an empty list instance of the specified type T. <br />
        /// 创建一个指定类型 T 的空列表实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Empty<T>() => new();

        #endregion

        #region ForEach

        /// <summary>
        /// Do action for each item, and returns the result.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(IEnumerable<T> source, Action<T> action)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            foreach (var item in source)
            {
                action?.Invoke(item);
                yield return item;
            }
        }

        #endregion

        #region IndexOf

        /// <summary>
        /// Index of
        /// </summary>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int IndexOf<T>(IEnumerable<T> source, T item)
        {
            return IndexOf(source, item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Index of
        /// </summary>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="equalityComparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int IndexOf<T>(IEnumerable<T> source, T item, IEqualityComparer<T> equalityComparer)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (equalityComparer is null)
                throw new ArgumentNullException(nameof(equalityComparer));

            return source.Select((i, index) => new {Item = i, Index = index})
                         .FirstOrDefault(p => equalityComparer.Equals(p.Item, item))
                         ?.Index ?? -1;
        }

        #endregion

        #region Move

        /// <summary>
        /// Move to first
        /// </summary>
        /// <param name="source"></param>
        /// <param name="element"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static List<TSource> MoveToFirst<TSource>(List<TSource> source, TSource element)
        {
            if (!source.Contains(element))
                return source;
            source.Remove(element);
            source.Insert(0, element);
            return source;
        }

        #endregion

        #region Order

        /// <summary>
        /// Make the collection random order<br />
        /// 打乱一个集合的顺序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> OrderByRandom<TSource>(IEnumerable<TSource> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            return source.OrderBy(_ => Guid.NewGuid());
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffle in place
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(IList<T> items)
        {
            Shuffle(items, 4);
        }

        /// <summary>
        /// Shuffle in place
        /// </summary>
        /// <param name="items"></param>
        /// <param name="times"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(IList<T> items, int times)
        {
            for (var j = 0; j < times; j++)
            {
                var rnd = new Random((int) (DateTime.Now.Ticks % int.MaxValue) - j);

                for (var i = 0; i < items.Count; i++)
                {
                    var index = rnd.Next(items.Count - 1);
                    var temp = items[index];
                    items[index] = items[i];
                    items[i] = temp;
                }
            }
        }

        /// <summary>
        /// Shuffle to new list
        /// </summary>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ShuffleAndNewInstance<T>(IList<T> items)
        {
            return ShuffleAndNewInstance(items, 4);
        }

        /// <summary>
        /// Shuffle to new list
        /// </summary>
        /// <param name="items"></param>
        /// <param name="times"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ShuffleAndNewInstance<T>(IList<T> items, int times)
        {
            var res = new List<T>(items);
            Shuffle(res, times);
            return res;
        }

        #endregion

        #region Take

        /// <summary>
        /// Take last
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> LastTake<T>(IEnumerable<T> source, int count)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, $"The {nameof(count)} parameter must be a non-negative number.");

            // If the count is zero, return empty.
            if (count == 0)
                return Enumerable.Empty<T>();

            // Start sniffing.
            // Read-only collection.
            if (source is IReadOnlyCollection<T> ro)
                return LinqCollsHelper.ReadOnlyCollectionTakeLast(ro, count);

            // Collection.
            if (source is ICollection<T> c)
                return LinqCollsHelper.CollectionTakeLast(c, count);

            // Default.
            return LinqCollsHelper.EnumerableTakeLast(source, count);
        }

        #endregion

        #region Unique Count

        /// <summary>
        /// Unique Count
        /// </summary>
        /// <param name="source"></param>
        /// <param name="valCalculator"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static int UniqueCount<TObj, TResult>(IList<TObj> source, Func<TObj, TResult> valCalculator)
        {
            var check = new HashSet<TResult>();

            foreach (var item in source)
            {
                var result = valCalculator(item);
                if (!check.Contains(result))
                    check.Add(result);
            }

            return check.Count;
        }

        #endregion
    }

    public static partial class CollsExtensions
    {
        #region BeContainedIn

        /// <summary>
        /// In
        /// </summary>
        /// <param name="item"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool BeContainedIn<T>(this T item, params T[] items)
        {
            return Colls.BeContainedIn(item, items, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// In
        /// </summary>
        /// <param name="item"></param>
        /// <param name="equalityComparer"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool BeContainedIn<T>(this T item, IEqualityComparer<T> equalityComparer, params T[] items)
        {
            return Colls.BeContainedIn(item, items, equalityComparer);
        }

        /// <summary>
        /// In
        /// </summary>
        /// <param name="item"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool BeContainedIn<T>(this T item, IEnumerable<T> items)
        {
            return Colls.BeContainedIn(item, items, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// In
        /// </summary>
        /// <param name="item"></param>
        /// <param name="items"></param>
        /// <param name="equalityComparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool BeContainedIn<T>(this T item, IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            return Colls.BeContainedIn(item, items, equalityComparer);
        }

        #endregion

        #region Contains

        /// <summary>
        /// Check whether the set contains members of the given condition.<br />
        /// 检查集合中是否包含给定条件的成员。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Contains<T>(this IEnumerable<T> source, Expression<Func<T, bool>> condition)
        {
            return Colls.Contains(source, condition);
        }

        /// <summary>
        /// Check if a set has a specified number of members. <br />
        /// 检查一个集合是否拥有指定数量的成员
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ContainsAtLeast<T>(this ICollection<T> source, int count)
        {
            return Colls.ContainsAtLeast(source, count);
        }

        /// <summary>
        /// Check if a set has a specified number of members. <br />
        /// 检查一个集合是否拥有指定数量的成员
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool ContainsAtLeast<T>(this IQueryable<T> source, int count)
        {
            return Colls.ContainsAtLeast(source, count);
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Do action for each item, and returns the result.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            return Colls.ForEach(source, action);
        }

        #endregion

        #region IndexOf

        /// <summary>
        /// Index of
        /// </summary>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T item)
        {
            return Colls.IndexOf(source, item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Index of
        /// </summary>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="equalityComparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int IndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> equalityComparer)
        {
            return Colls.IndexOf(source, item, equalityComparer);
        }

        #endregion

        #region Order

        /// <summary>
        /// Make the collection random order<br />
        /// 打乱一个集合的顺序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> OrderByRandom<TSource>(this IEnumerable<TSource> source)
        {
            return Colls.OrderByRandom(source);
        }

        #endregion

        #region Take

        /// <summary>
        /// Take last
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<T> LastTake<T>(this IEnumerable<T> source, int count)
        {
            return Colls.LastTake(source, count);
        }

        #endregion

        #region Unique Count

        /// <summary>
        /// Unique Count
        /// </summary>
        /// <param name="source"></param>
        /// <param name="valCalculator"></param>
        /// <typeparam name="TObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static int UniqueCount<TObj, TResult>(this IList<TObj> source, Func<TObj, TResult> valCalculator)
        {
            return Colls.UniqueCount(source, valCalculator);
        }

        #endregion
    }
}