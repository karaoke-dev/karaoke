// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LyricUtils
    {
        public static Tuple<Lyric, Lyric> SplitLyric(Lyric lyric)
        {
            // todo : should create two lyric.
            return new Tuple<Lyric, Lyric>(new Lyric(), new Lyric());
        }

        public static Lyric CombineLyric(Lyric lyric1, Lyric lyric2)
        {
            // todo : should create lyric and copy those property.
            return new Lyric
            {
                Text = lyric1.Text + lyric2.Text,
            };
        }

        public static IEnumerable<Note> CreateDefaultNotes(Lyric lyric)
        {
            var timeTags = TimeTagsUtils.ToDictionary(lyric.TimeTags);

            foreach (var timeTag in timeTags)
            {
                var (key, endTime) = timeTags.GetNext(timeTag);

                if (key.Index <= 0)
                    continue;

                var startTime = timeTag.Value;

                int startIndex = timeTag.Key.Index;
                int endIndex = key.Index;

                var text = lyric.Text.Substring(startIndex, endIndex - startIndex);
                var ruby = lyric.RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault().Text;

                if (!string.IsNullOrEmpty(text))
                {
                    yield return new Note
                    {
                        StartTime = startTime,
                        Duration = endTime - startTime,
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        Text = text,
                        AlternativeText = ruby,
                        ParentLyric = lyric
                    };
                }
            }
        }
    }
}
