// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using WanaKanaSharp;
using Zipangu;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class JpStringUtils
    {
        public static string ToHiragana(string text)
        {
            return text.KatakanaToHiragana();
        }

        public static string ToKatakana(string text)
        {
            return text.HiraganaToKatakana();
        }

        public static string ToRomaji(string text)
        {
            return RomajiConverter.ToRomaji(text, false, null);
        }
    }
}
