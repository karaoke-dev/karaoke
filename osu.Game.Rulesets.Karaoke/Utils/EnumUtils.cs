// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class EnumUtils
    {
        public static T[] GetValues<T>() where T : struct
            => (T[])Enum.GetValues(typeof(T));

        public static T GetPreviousValue<T>(T v) where T : struct
            => GetValues<T>().Concat(new[] { default(T) }).Reverse().SkipWhile(e => !EqualityComparer<T>.Default.Equals(v, e)).Skip(1).First();

        public static T GetNextValue<T>(T v) where T : struct
            => GetValues<T>().Concat(new[] { default(T) }).SkipWhile(e => !EqualityComparer<T>.Default.Equals(v, e)).Skip(1).First();
    }
}
