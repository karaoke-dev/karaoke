// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class SingerUtils
    {
        public static int GetShiftingStyleIndex(IEnumerable<int> singerIds)
            => singerIds?.Sum(x => (int)Math.Pow(2, x - 1)) ?? 0;

        public static int[] GetSingersIndex(int styleIndex)
        {
            if (styleIndex < 1)
                return Array.Empty<int>();

            string binary = Convert.ToString(styleIndex, 2);

            return binary.Select((v, i) => new { value = v, singer = binary.Length - i })
                         .Where(x => x.value == '1').Select(x => x.singer).OrderBy(x => x).ToArray();
        }
    }
}
