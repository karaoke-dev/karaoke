// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Retrieves the item after a pivot from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items stored in the collection.</typeparam>
        /// <param name="collection">The collection to iterate on.</param>
        /// <param name="pivot">The pivot value.</param>
        /// <param name="action">Match action</param>
        /// <returns>The item in <paramref name="collection"/> appearing after <paramref name="pivot"/>, or null if no such item exists.</returns>
        public static T? GetNextMatch<T>(this IEnumerable<T> collection, T pivot, Func<T, bool> action) where T : notnull
        {
            return collection.SkipWhile(i => !EqualityComparer<T>.Default.Equals(i, pivot)).Skip(1).SkipWhile(x => !action(x)).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the item before a pivot from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items stored in the collection.</typeparam>
        /// <param name="collection">The collection to iterate on.</param>
        /// <param name="pivot">The pivot value.</param>
        /// <param name="action">Match action</param>
        /// <returns>The item in <paramref name="collection"/> appearing before <paramref name="pivot"/>, or null if no such item exists.</returns>
        public static T? GetPreviousMatch<T>(this IEnumerable<T> collection, T pivot, Func<T, bool> action) where T : notnull
        {
            return collection.Reverse().SkipWhile(i => !EqualityComparer<T>.Default.Equals(i, pivot)).Skip(1).SkipWhile(x => !action(x)).FirstOrDefault();
        }

        /// <summary>
        /// Convert [][] to [,]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var data = source
                       .Select(x => x.ToArray())
                       .ToArray();

            var res = new T[data.Length, data.Max(x => x.Length)];

            for (int i = 0; i < data.Length; ++i)
            {
                for (int j = 0; j < data[i].Length; ++j)
                {
                    res[i, j] = data[i][j];
                }
            }

            return res;
        }

        public static int IndexOf<T>(this IEnumerable<T> array, T value)
        {
            return array.ToList().IndexOf(value);
        }
    }
}
