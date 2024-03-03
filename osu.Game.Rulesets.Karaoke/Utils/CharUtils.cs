// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.Unicode;

namespace osu.Game.Rulesets.Karaoke.Utils;

public static class CharUtils
{
    public static bool IsSpacing(char c)
        => c is ' ' or '　';

    /// <summary>
    /// Check this char is kana
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsKana(char c) =>
        (c >= '\u3041' && c <= '\u309F') | // ひらがなwith゛゜
        (c >= '\u30A0' && c <= '\u30FF') | // カタカナwith゠・ー
        (c >= '\u31F0' && c <= '\u31FF') | // Katakana Phonetic Extensions
        (c >= '\uFF65' && c <= '\uFF9F');

    /// <summary>
    /// Check this character is English latter or not.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsEnglish(char c) =>
        (c >= 'A' && c <= 'Z') ||
        (c >= 'a' && c <= 'z') ||
        (c >= 'Ａ' && c <= 'Ｚ') ||
        (c >= 'ａ' && c <= 'ｚ');

    /// <summary>
    /// Check this char is symbol
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsAsciiSymbol(char c) =>
        (c >= ' ' && c <= '/') ||
        (c >= ':' && c <= '@') ||
        (c >= '[' && c <= '`') ||
        (c >= '{' && c <= '~');

    /// <summary>
    /// Check this char is chinese character
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsChinese(char c)
    {
        // From : https://stackoverflow.com/a/61738863/4105113
        int minValue = UnicodeRanges.CjkUnifiedIdeographs.FirstCodePoint;
        int maxValue = minValue + UnicodeRanges.CjkUnifiedIdeographs.Length;
        return c >= minValue && c < maxValue;
    }

    /// <summary>
    /// Check this char is latin alphabet or not.
    /// Usually, this is used to check the romanisation result.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsLatin(char c)
    {
        switch (c)
        {
            case >= 'A' and <= 'Z':
            case >= 'a' and <= 'z':
            // another romanized characters
            // see: https://www.unicode.org/charts/PDF/U1E00.pdf
            case >= '\u1E00' and <= '\u1EFF':
                return true;

            default:
                return false;
        }
    }
}
