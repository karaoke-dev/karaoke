// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextTagUtils
    {
        public static T FixTimeTagPosition<T>(T textTag) where T : ITextTag
        {
            int startIndex = Math.Min(textTag.StartIndex, textTag.EndIndex);
            int endIndex = Math.Max(textTag.StartIndex, textTag.EndIndex);

            textTag.StartIndex = startIndex;
            textTag.EndIndex = endIndex;
            return textTag;
        }

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

            var fixedTextTag = FixTimeTagPosition(textTag);
            return fixedTextTag.StartIndex < 0 || fixedTextTag.EndIndex > lyric.Length;
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
            var fixedTag = FixTimeTagPosition(textTag);
            string text = string.IsNullOrWhiteSpace(fixedTag.Text) ? "empty" : fixedTag.Text;
            return $"{text}({fixedTag.StartIndex} ~ {fixedTag.EndIndex})";
        }

        public static string GetTextFromLyric<T>(T textTag, string lyric) where T : ITextTag
        {
            if (textTag == null || lyric == null)
                return null;

            var fixedTextTag = FixTimeTagPosition(textTag);
            int startIndex = Math.Max(0, fixedTextTag.StartIndex);
            int endIndex = Math.Min(lyric.Length, fixedTextTag.EndIndex);
            return lyric.Substring(startIndex, endIndex - startIndex);
        }
    }
}
