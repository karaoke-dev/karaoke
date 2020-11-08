// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class SingerUtils
    {
        public static int GetShiftingStyleIndex(int[] singerIds)
            => singerIds.Sum(x => (int)Math.Pow(2, x - 1));

        public static int[] GetSingersIndex(int styleIndex)
            => throw new NotImplementedException();
    }
}
