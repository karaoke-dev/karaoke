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

        public static Lyric CombineLyric(Lyric fisrtLyric, Lyric secondLyric)
        {
            if (fisrtLyric == null || secondLyric == null)
                throw new ArgumentNullException($"{nameof(fisrtLyric)} or {nameof(secondLyric)} cannot be null.");

            var shiftingIndex = fisrtLyric.Text?.Length ?? 0;

            var timeTags = new List<TimeTag>();
            timeTags.AddRangeWithNullCheck(fisrtLyric.TimeTags);
            timeTags.AddRangeWithNullCheck(shiftingTimeTag(secondLyric.TimeTags, shiftingIndex));

            var rubyTags = new List<RubyTag>();
            rubyTags.AddRangeWithNullCheck(fisrtLyric.RubyTags);
            rubyTags.AddRangeWithNullCheck(shiftingRubyTag(secondLyric.RubyTags, shiftingIndex));

            var romajiTags = new List<RomajiTag>();
            romajiTags.AddRangeWithNullCheck(fisrtLyric.RomajiTags);
            romajiTags.AddRangeWithNullCheck(shiftingRomajiTag(secondLyric.RomajiTags, shiftingIndex));

            var startTime = Math.Min(fisrtLyric.StartTime, secondLyric.StartTime);
            var endTime = Math.Max(fisrtLyric.EndTime, secondLyric.EndTime);

            var singers = new List<int>();
            singers.AddRangeWithNullCheck(fisrtLyric.Singers);
            singers.AddRangeWithNullCheck(secondLyric.Singers);

            var sameLanguage = fisrtLyric.Language?.Equals(secondLyric.Language) ?? false;
            var language = sameLanguage ? fisrtLyric.Language : null;

            return new Lyric
            {
                Text = fisrtLyric.Text + secondLyric.Text,
                TimeTags = timeTags.ToArray(),
                RubyTags = rubyTags.ToArray(),
                RomajiTags = romajiTags.ToArray(),
                StartTime = startTime,
                Duration = endTime - startTime,
                Singers = singers.Distinct().ToArray(),
                LayoutIndex = fisrtLyric.LayoutIndex,
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
