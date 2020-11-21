// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Zipangu;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public class JpStringUtils
    {
        public static string ToHiragana(string text)
        {
            return text.KatakanaToHiragana();
        }

        public static string ToKatakana(string text)
        {
            return text.HiraganaToKatakana();
        }
    }
}
