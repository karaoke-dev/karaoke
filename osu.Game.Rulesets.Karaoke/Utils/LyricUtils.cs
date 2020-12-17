// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LyricUtils
    {
        #region progessing

        public static Tuple<Lyric, Lyric> SplitLyric(Lyric lyric, int splitIndex)
        {
            if (lyric == null)
                throw new ArgumentNullException($"{nameof(lyric)} cannot be null.");

            if (string.IsNullOrEmpty(lyric.Text))
                throw new ArgumentNullException($"{nameof(lyric.Text)} cannot be null.");

            if (splitIndex < 0 || splitIndex > lyric.Text.Length)
                throw new ArgumentOutOfRangeException(nameof(splitIndex));

            if (splitIndex == 0 || splitIndex == lyric.Text.Length)
                throw new InvalidOperationException($"{nameof(splitIndex)} cannot cut at first or last index.");

            var firstTimeTag = lyric.TimeTags?.Where(x => x.Index.Index < splitIndex).ToList();
            var secondTimeTag = lyric.TimeTags?.Where(x => x.Index.Index >= splitIndex).ToList();

            // add delta time-tag if does not have end time-tag.
            if (firstTimeTag?.Count > 0 && secondTimeTag.Count > 0)
            {
                var firstTag = firstTimeTag.LastOrDefault();
                var secondTag = secondTimeTag.FirstOrDefault();

                // add end tag at end of first lyric if does not have tag in there.
                if (!firstTimeTag.Any(x => x.Index.Index == splitIndex - 1 && x.Index.State == TimeTagIndex.IndexState.End))
                {
                    var endTagIndex = new TimeTagIndex(splitIndex - 1, TimeTagIndex.IndexState.End);
                    var endTag = TimeTagsUtils.GenerateCenterTimeTag(firstTag, secondTag, endTagIndex);
                    firstTimeTag.Add(endTag);
                }

                // add start tag at start of second lyric if does not have tag in there.
                if (!secondTimeTag.Any(x => x.Index.Index == splitIndex && x.Index.State == TimeTagIndex.IndexState.Start))
                {
                    var endTagIndex = new TimeTagIndex(splitIndex, TimeTagIndex.IndexState.Start);
                    var startTag = TimeTagsUtils.GenerateCenterTimeTag(firstTag, secondTag, endTagIndex);
                    secondTimeTag.Add(startTag);
                }
            }

            var firstLyric = new Lyric
            {
                Text = lyric.Text?.Substring(0, splitIndex),
                TimeTags = firstTimeTag?.ToArray(),
                RubyTags = lyric.RubyTags?.Where(x => x.StartIndex < splitIndex && x.EndIndex <= splitIndex).ToArray(),
                RomajiTags = lyric.RomajiTags?.Where(x => x.StartIndex < splitIndex && x.EndIndex <= splitIndex).ToArray(),
                // todo : should implement time and duration
                Singers = lyric.Singers?.Clone() as int[],
                LayoutIndex = lyric.LayoutIndex,
                Language = lyric.Language,
            };

            var secondLyric = new Lyric
            {
                Text = lyric.Text?.Substring(splitIndex),
                TimeTags = shiftingTimeTag(secondTimeTag?.ToArray(), -splitIndex),
                RubyTags = shiftingRubyTag(lyric.RubyTags?.Where(x => x.StartIndex >= splitIndex && x.EndIndex > splitIndex).ToArray(), -splitIndex),
                RomajiTags = shiftingRomajiTag(lyric.RomajiTags?.Where(x => x.StartIndex >= splitIndex && x.EndIndex > splitIndex).ToArray(), -splitIndex),
                // todo : should implement time and duration
                Singers = lyric.Singers?.Clone() as int[],
                LayoutIndex = lyric.LayoutIndex,
                Language = lyric.Language,
            };

            return new Tuple<Lyric, Lyric>(firstLyric, secondLyric);
        }

        public static Lyric CombineLyric(Lyric firstLyric, Lyric secondLyric)
        {
            if (firstLyric == null || secondLyric == null)
                throw new ArgumentNullException($"{nameof(firstLyric)} or {nameof(secondLyric)} cannot be null.");

            var shiftingIndex = firstLyric.Text?.Length ?? 0;

            var timeTags = new List<TimeTag>();
            timeTags.AddRangeWithNullCheck(firstLyric.TimeTags);
            timeTags.AddRangeWithNullCheck(shiftingTimeTag(secondLyric.TimeTags, shiftingIndex));

            var rubyTags = new List<RubyTag>();
            rubyTags.AddRangeWithNullCheck(firstLyric.RubyTags);
            rubyTags.AddRangeWithNullCheck(shiftingRubyTag(secondLyric.RubyTags, shiftingIndex));

            var romajiTags = new List<RomajiTag>();
            romajiTags.AddRangeWithNullCheck(firstLyric.RomajiTags);
            romajiTags.AddRangeWithNullCheck(shiftingRomajiTag(secondLyric.RomajiTags, shiftingIndex));

            var startTime = Math.Min(firstLyric.StartTime, secondLyric.StartTime);
            var endTime = Math.Max(firstLyric.EndTime, secondLyric.EndTime);

            var singers = new List<int>();
            singers.AddRangeWithNullCheck(firstLyric.Singers);
            singers.AddRangeWithNullCheck(secondLyric.Singers);

            var sameLanguage = firstLyric.Language?.Equals(secondLyric.Language) ?? false;
            var language = sameLanguage ? firstLyric.Language : null;

            return new Lyric
            {
                Text = firstLyric.Text + secondLyric.Text,
                TimeTags = timeTags.ToArray(),
                RubyTags = rubyTags.ToArray(),
                RomajiTags = romajiTags.ToArray(),
                StartTime = startTime,
                Duration = endTime - startTime,
                Singers = singers.Distinct().ToArray(),
                LayoutIndex = firstLyric.LayoutIndex,
                Language = language,
            };
        }

        public static void RemoveText(Lyric lyric, int position, int count = 1)
        {
            if (lyric == null)
                throw new ArgumentNullException($"{nameof(lyric)} cannot be null.");

            var textLength = lyric.Text.Length;
            if (textLength == 0)
                return;

            if (position < 0 || position > textLength)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(position));

            if (position + count >= textLength)
                count = textLength - position;

            // deal with ruby and romaji, might remove and shifting.
            lyric.RubyTags = processTags(lyric.RubyTags, position, count);
            lyric.RomajiTags = processTags(lyric.RomajiTags, position, count);

            // deal with text
            var newLyric = lyric.Text.Substring(0, position) + lyric.Text[(position + count)..];
            lyric.Text = newLyric;

            static T[] processTags<T>(T[] tags, int position, int count = 1) where T : ITextTag
            {
                var endPosition = position + count;
                return tags?.SkipWhile(x => x.StartIndex >= position && x.EndIndex <= endPosition)
                .Select(x =>
                {
                    if (x.StartIndex < position)
                        x.EndIndex = Math.Min(x.EndIndex, position);
                    if (x.EndIndex > position)
                    {
                        x.StartIndex = Math.Max(x.StartIndex, endPosition) - count;
                        x.EndIndex -= count;
                    }
                    return x;
                })
                .ToArray();
            }
        }

        public static void AddText(Lyric lyric, int position, string text)
        {

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

        private static RomajiTag[] shiftingRomajiTag(RomajiTag[] romajiTags, int shiftingIndex)
            => romajiTags?.Select(t => new RomajiTag
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
