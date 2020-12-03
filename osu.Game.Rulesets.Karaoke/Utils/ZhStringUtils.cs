// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.Unicode;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class ZhStringUtils
    {
        public static bool IsChinese(char character)
        {
            // From : https://stackoverflow.com/a/61738863/4105113
            var minValue = UnicodeRanges.CjkUnifiedIdeographs.FirstCodePoint;
            var maxValue = minValue + UnicodeRanges.CjkUnifiedIdeographs.Length;
            return character >= minValue && character < maxValue;
        }
    }
}
