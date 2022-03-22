// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osuTK;
using osuTK.Graphics;

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

        public static Color4 GetContentColour(ISinger singer)
            => Color4.FromHsl(new Vector4(singer.Hue / 360, 0.4f, 0.6f, 1));

        public static Color4 GetBackgroundColour(ISinger singer)
            => Color4.FromHsl(new Vector4(singer.Hue / 360, 0.1f, 0.4f, 1));
    }
}
