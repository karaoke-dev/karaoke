// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value)
        {
            return array.ToList().IndexOf(value);
        }
    }
}
