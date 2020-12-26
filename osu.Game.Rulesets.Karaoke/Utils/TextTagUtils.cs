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

        public static T Shifting<T>(T textTag, int shifting) where T : ITextTag, new()
        {
            return new T
            {
                StartIndex = textTag.StartIndex + shifting,
                EndIndex = textTag.EndIndex + shifting,
                Text = textTag.Text
            };
        }
    }
}
