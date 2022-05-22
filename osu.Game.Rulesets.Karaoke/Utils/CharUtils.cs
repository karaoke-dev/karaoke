// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.Unicode;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class CharUtils
    {
        public static bool IsSpacing(char c)
            => c is ' ' or '　';

        /// <summary>
        /// Check this char is kana
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsKana(char c)
        {
            return (c >= '\u3041' && c <= '\u309F') | // ひらがなwith゛゜
                   (c >= '\u30A0' && c <= '\u30FF') | // カタカナwith゠・ー
                   (c >= '\u31F0' && c <= '\u31FF') | // Katakana Phonetic Extensions
                   (c >= '\uFF65' && c <= '\uFF9F');
        }

        /// <summary>
        /// Check this character is english
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLatin(char c)
        {
            return c >= 'A' && c <= 'Z' ||
                   c >= 'a' && c <= 'z' ||
                   c >= 'Ａ' && c <= 'Ｚ' ||
                   c >= 'ａ' && c <= 'ｚ';
        }

        /// <summary>
        /// Check this char is symbol
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAsciiSymbol(char c)
        {
            return c >= ' ' && c <= '/' ||
                   c >= ':' && c <= '@' ||
                   c >= '[' && c <= '`' ||
                   c >= '{' && c <= '~';
        }

        /// <summary>
        /// Check this char is chinese character
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool IsChinese(char character)
        {
            // From : https://stackoverflow.com/a/61738863/4105113
            int minValue = UnicodeRanges.CjkUnifiedIdeographs.FirstCodePoint;
            int maxValue = minValue + UnicodeRanges.CjkUnifiedIdeographs.Length;
            return character >= minValue && character < maxValue;
        }
    }
}
