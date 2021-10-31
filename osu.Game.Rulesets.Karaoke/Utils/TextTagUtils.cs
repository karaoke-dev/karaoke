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
            var startIndex = Math.Min(textTag.StartIndex, textTag.EndIndex);
            var endIndex = Math.Max(textTag.StartIndex, textTag.EndIndex);

            textTag.StartIndex = startIndex;
            textTag.EndIndex = endIndex;
            return textTag;
        }

        public static T Shifting<T>(T textTag, int shifting) where T : ITextTag, new() =>
            new()
            {
                StartIndex = textTag.StartIndex + shifting,
                EndIndex = textTag.EndIndex + shifting,
                Text = textTag.Text
            };

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
            var text = string.IsNullOrWhiteSpace(fixedTag.Text) ? "empty" : fixedTag.Text;
            return $"{text}({fixedTag.StartIndex} ~ {fixedTag.EndIndex})";
        }

        public static string GetTextFromLyric<T>(T textTag, string lyric) where T : ITextTag
        {
            if (textTag == null || lyric == null)
                return null;

            var fixedTextTag = FixTimeTagPosition(textTag);
            var startIndex = Math.Max(0, fixedTextTag.StartIndex);
            var endIndex = Math.Min(lyric.Length, fixedTextTag.EndIndex);
            return lyric.Substring(startIndex, endIndex - startIndex);
        }
    }
}
