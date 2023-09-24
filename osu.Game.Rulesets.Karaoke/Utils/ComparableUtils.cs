// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils;

public static class ComparableUtils
{
    public static int Compare<T>(T? x, T? y, params Func<T, T, int>[] comparers)
    {
        if (x == null || y == null)
            throw new InvalidOperationException("This utils does not support null cases");

        return comparers.Select(cmp => cmp(x, y))
                        .FirstOrDefault(result => result != 0);
    }

    public static int CompareByProperty<T>(T? x, T? y, params Func<T, object>[] comparers)
    {
        var comparerResults = comparers.Select(comparer =>
        {
            return (Func<T, T, int>)compareFunction;

            int compareFunction(T aa, T bb)
            {
                object xPropertyValue = comparer(aa);
                object yPropertyValue = comparer(bb);
                return Comparer.Default.Compare(xPropertyValue, yPropertyValue);
            }
        }).ToArray();

        return Compare(x, y, comparerResults);
    }
}
