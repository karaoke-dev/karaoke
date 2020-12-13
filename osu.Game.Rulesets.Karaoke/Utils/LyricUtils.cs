// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LyricUtils
    {
        #region progessing

        public static Tuple<Lyric, Lyric> SplitLyric(Lyric lyric, int splitIndex)
        {
            // todo : should create two lyric.
            return new Tuple<Lyric, Lyric>(new Lyric(), new Lyric());
        }

        public static Lyric CombineLyric(Lyric lyric1, Lyric lyric2)
        {
            var shiftingIndex = lyric1.Text?.Length ?? 0;

            var timeTags = new List<TimeTag>();
            timeTags.AddRangeWithNullCheck(lyric1.TimeTags);
            timeTags.AddRangeWithNullCheck(shiftingTimeTag(lyric2.TimeTags, shiftingIndex));

            var rubyTags = new List<RubyTag>();
            rubyTags.AddRangeWithNullCheck(lyric1.RubyTags);
            rubyTags.AddRangeWithNullCheck(shiftingRubyTag(lyric2.RubyTags, shiftingIndex));

            var romajiTags = new List<RomajiTag>();
            romajiTags.AddRangeWithNullCheck(lyric1.RomajiTags);
            romajiTags.AddRangeWithNullCheck(shiftingRomajiTag(lyric2.RomajiTags, shiftingIndex));

            var startTime = Math.Min(lyric1.StartTime, lyric2.StartTime);
            var endTime = Math.Max(lyric1.EndTime, lyric2.EndTime);

            var singers = new List<int>();
            singers.AddRangeWithNullCheck(lyric1.Singers);
            singers.AddRangeWithNullCheck(lyric2.Singers);

            var sameLanguage = lyric1.Language?.Equals(lyric2.Language) ?? false;
            var language = sameLanguage ? lyric1.Language : null;

            return new Lyric
            {
                Text = lyric1.Text + lyric2.Text,
                TimeTags = timeTags.ToArray(),
                RubyTags = rubyTags.ToArray(),
                RomajiTags = romajiTags.ToArray(),
                StartTime = startTime,
                Duration = endTime - startTime,
                Singers = singers.Distinct().ToArray(),
                LayoutIndex = lyric1.LayoutIndex,
                Language = language,
            };
        }

        private static TimeTag[] shiftingTimeTag(TimeTag[] timeTags, int shiftingIndex)
            => timeTags?.Select(t => new TimeTag(new TimeTagIndex(t.Index.Index + shiftingIndex, t.Index.State), t.Time)).ToArray();

        private static RubyTag[] shiftingRubyTag(RubyTag[] rubyTags, int shiftingIndex)
            => rubyTags?.Select(t => new RubyTag
            {
                StartIndex = t.StartIndex + shiftingIndex,
                EndIndex = t.EndIndex + shiftingIndex,
                Text = t.Text
            }).ToArray();

        private static RomajiTag[] shiftingRomajiTag(RomajiTag[] rubyTags, int shiftingIndex)
            => rubyTags?.Select(t => new RomajiTag
            {
                StartIndex = t.StartIndex + shiftingIndex,
                EndIndex = t.EndIndex + shiftingIndex,
                Text = t.Text
            }).ToArray();

        #endregion

        #region create default

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

        #endregion
    }
}
