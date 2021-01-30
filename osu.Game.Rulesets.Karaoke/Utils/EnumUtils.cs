// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class EnumUtils
    {
        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
