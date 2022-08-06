// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper
{
    public static class TestCaseNoteHelper
    {
        public static Lyric CreateLyricForNote(string text, double startTime, double duration)
        {
            return new Lyric
            {
                Text = text,
                TimeTags = new List<TimeTag>
                {
                    new(new TextIndex(0), startTime),
                    new(new TextIndex(text.Length - 1, TextIndex.IndexState.End), startTime + duration)
                }
            };
        }
    }
}
