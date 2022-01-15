// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagUtils
    {
        public static Tuple<int, int> GetFixedIndex<T>(T textTag, string lyric) where T : ITextTag
            => GetShiftingIndex(textTag, lyric, 0);

        public static Tuple<int, int> GetShiftingIndex<T>(T textTag, string lyric, int shifting) where T : ITextTag
        {
            int lyricLength = lyric?.Length ?? 0;
            int newStartIndex = Math.Clamp(textTag.StartIndex + shifting, 0, lyricLength);
            int newEndIndex = Math.Clamp(textTag.EndIndex + shifting, 0, lyricLength);
            return new Tuple<int, int>(Math.Min(newStartIndex, newEndIndex), Math.Max(newStartIndex, newEndIndex));
        }

        public static bool OutOfRange<T>(T textTag, string lyric) where T : ITextTag
        {
            if (string.IsNullOrEmpty(lyric))
                return true;

            return outOfRange(lyric, textTag.StartIndex) || outOfRange(lyric, textTag.EndIndex);

            static bool outOfRange(string lyric, int index)
            {
                const int min_index = 0;
                int maxIndex = lyric.Length;

                return index < min_index || index > maxIndex;
            }
        }

        public static bool EmptyText<T>(T textTag) where T : ITextTag
        {
            return string.IsNullOrEmpty(textTag.Text);
        }

        /// <summary>
        /// Display tag with position format
        /// </summary>
        /// <example>
        /// ka(-2~-1)
        /// ra(4~6)
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="textTag"></param>
        /// <returns></returns>
        public static string PositionFormattedString<T>(T textTag) where T : ITextTag
        {
            string text = string.IsNullOrWhiteSpace(textTag.Text) ? "empty" : textTag.Text;
            return $"{text}({textTag.StartIndex} ~ {textTag.EndIndex})";
        }

        public static string GetTextFromLyric<T>(T textTag, string lyric) where T : ITextTag
        {
            if (textTag == null || lyric == null)
                return null;

            (int startIndex, int endIndex) = GetFixedIndex(textTag, lyric);
            return lyric.Substring(startIndex, endIndex - startIndex);
        }
    }
}
