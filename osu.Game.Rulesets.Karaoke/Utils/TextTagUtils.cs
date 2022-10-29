// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagUtils
    {
        public static Tuple<int, int> GetFixedIndex<T>(T textTag, string lyric) where T : ITextTag
            => GetShiftingIndex(textTag, lyric, 0);

        public static Tuple<int, int> GetShiftingIndex<T>(T textTag, string lyric, int offset) where T : ITextTag
        {
            int lyricLength = lyric.Length;
            int newStartIndex = Math.Clamp(textTag.StartIndex + offset, 0, lyricLength);
            int newEndIndex = Math.Clamp(textTag.EndIndex + offset, 0, lyricLength);
            return new Tuple<int, int>(Math.Min(newStartIndex, newEndIndex), Math.Max(newStartIndex, newEndIndex));
        }

        public static bool OutOfRange<T>(T textTag, string lyric) where T : ITextTag
        {
            return OutOfRange(lyric, textTag.StartIndex) || OutOfRange(lyric, textTag.EndIndex);
        }

        public static bool ValidNewStartIndex<T>(T textTag, int newStartIndex) where T : ITextTag
        {
            if (textTag == null)
                throw new ArgumentNullException(nameof(textTag));

            return newStartIndex < textTag.EndIndex;
        }

        public static bool ValidNewEndIndex<T>(T textTag, int newEndIndex) where T : ITextTag
        {
            if (textTag == null)
                throw new ArgumentNullException(nameof(textTag));

            return newEndIndex > textTag.StartIndex;
        }

        public static bool OutOfRange(string lyric, int index)
        {
            if (string.IsNullOrEmpty(lyric))
                return true;

            const int min_index = 0;
            int maxIndex = lyric.Length;

            return index < min_index || index > maxIndex;
        }

        public static bool EmptyText<T>(T textTag) where T : ITextTag
        {
            return string.IsNullOrWhiteSpace(textTag.Text);
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
            (int startIndex, int endIndex) = GetFixedIndex(textTag, lyric);
            return lyric.Substring(startIndex, endIndex - startIndex);
        }

        public static PositionText ToPositionText<T>(T textTag) where T : ITextTag
            => new(textTag.Text, textTag.StartIndex, textTag.EndIndex);
    }
}
