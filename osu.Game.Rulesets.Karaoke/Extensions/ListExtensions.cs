// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class ListExtensions
    {
        public static void AddRangeWithNullCheck<T>(this List<T> collection, IEnumerable<T> newValue)
        {
            if (newValue == null)
                return;

            collection.AddRange(newValue);
        }
    }
}
