// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LyricsUtils
    {
        #region processing

        public static Tuple<Lyric, Lyric> SplitLyric(Lyric lyric, int splitIndex)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            string lyricText = lyric.Text;
            if (string.IsNullOrEmpty(lyricText))
                throw new ArgumentNullException(nameof(lyricText));

            if (splitIndex < 0 || splitIndex > lyricText.Length)
                throw new ArgumentOutOfRangeException(nameof(splitIndex));

            if (splitIndex == 0 || splitIndex == lyricText.Length)
                throw new InvalidOperationException($"{nameof(splitIndex)} cannot cut at first or last index.");

            var firstTimeTag = lyric.TimeTags.Where(x => x.Index.Index < splitIndex).ToList();
            var secondTimeTag = lyric.TimeTags.Where(x => x.Index.Index >= splitIndex).ToList();

            // add delta time-tag if does not have end time-tag.
            if (firstTimeTag.Count > 0 && secondTimeTag.Count > 0)
            {
                var firstTag = firstTimeTag.LastOrDefault();
                var secondTag = secondTimeTag.FirstOrDefault();

                if (firstTag != null && secondTag != null)
                {
                    // add end tag at end of first lyric if does not have tag in there.
                    if (!firstTimeTag.Any(x => x.Index.Index == splitIndex - 1 && x.Index.State == TextIndex.IndexState.End))
                    {
                        var endTagIndex = new TextIndex(splitIndex - 1, TextIndex.IndexState.End);
                        var endTag = TimeTagsUtils.GenerateCenterTimeTag(firstTag, secondTag, endTagIndex);
                        firstTimeTag.Add(endTag);
                    }

                    // add start tag at start of second lyric if does not have tag in there.
                    if (!secondTimeTag.Any(x => x.Index.Index == splitIndex && x.Index.State == TextIndex.IndexState.Start))
                    {
                        var endTagIndex = new TextIndex(splitIndex);
                        var startTag = TimeTagsUtils.GenerateCenterTimeTag(firstTag, secondTag, endTagIndex);
                        secondTimeTag.Add(startTag);
                    }
                }
            }

            // todo : should implement time and duration
            var firstLyric = lyric.DeepClone();
            firstLyric.Text = lyric.Text[..splitIndex];
            firstLyric.TimeTags = firstTimeTag.ToArray();
            firstLyric.RubyTags = lyric.RubyTags.Where(x => x.StartIndex < splitIndex && x.EndIndex <= splitIndex).ToArray();
            firstLyric.RomajiTags = lyric.RomajiTags.Where(x => x.StartIndex < splitIndex && x.EndIndex <= splitIndex).ToArray();

            // todo : should implement time and duration
            string secondLyricText = lyric.Text[splitIndex..];
            var secondLyric = lyric.DeepClone();
            secondLyric.Text = secondLyricText;
            secondLyric.TimeTags = shiftingTimeTag(secondTimeTag.ToArray(), -splitIndex);
            secondLyric.RubyTags = shiftingTextTag(lyric.RubyTags.Where(x => x.StartIndex >= splitIndex && x.EndIndex > splitIndex).ToArray(), secondLyricText, -splitIndex);
            secondLyric.RomajiTags = shiftingTextTag(lyric.RomajiTags.Where(x => x.StartIndex >= splitIndex && x.EndIndex > splitIndex).ToArray(), secondLyricText, -splitIndex);

            return new Tuple<Lyric, Lyric>(firstLyric, secondLyric);
        }

        public static Lyric CombineLyric(Lyric firstLyric, Lyric secondLyric)
        {
            if (firstLyric == null)
                throw new ArgumentNullException(nameof(firstLyric));

            if (secondLyric == null)
                throw new ArgumentNullException(nameof(secondLyric));

            int offsetIndexForSecondLyric = firstLyric.Text.Length;
            string lyricText = firstLyric.Text + secondLyric.Text;

            var timeTags = new List<TimeTag>();
            timeTags.AddRangeWithNullCheck(firstLyric.TimeTags);
            timeTags.AddRangeWithNullCheck(shiftingTimeTag(secondLyric.TimeTags, offsetIndexForSecondLyric));

            var rubyTags = new List<RubyTag>();
            rubyTags.AddRangeWithNullCheck(firstLyric.RubyTags);
            rubyTags.AddRangeWithNullCheck(shiftingTextTag(secondLyric.RubyTags, lyricText, offsetIndexForSecondLyric));

            var romajiTags = new List<RomajiTag>();
            romajiTags.AddRangeWithNullCheck(firstLyric.RomajiTags);
            romajiTags.AddRangeWithNullCheck(shiftingTextTag(secondLyric.RomajiTags, lyricText, offsetIndexForSecondLyric));

            double startTime = Math.Min(firstLyric.StartTime, secondLyric.StartTime);
            double endTime = Math.Max(firstLyric.EndTime, secondLyric.EndTime);

            var singers = new List<int>();
            singers.AddRangeWithNullCheck(firstLyric.Singers);
            singers.AddRangeWithNullCheck(secondLyric.Singers);

            bool sameLanguage = EqualityComparer<CultureInfo?>.Default.Equals(firstLyric.Language, secondLyric.Language);
            var language = sameLanguage ? firstLyric.Language : null;

            return new Lyric
            {
                Text = lyricText,
                TimeTags = timeTags.ToArray(),
                RubyTags = rubyTags.ToArray(),
                RomajiTags = romajiTags.ToArray(),
                StartTime = startTime,
                Duration = endTime - startTime,
                Singers = singers.Distinct().ToArray(),
                Language = language,
            };
        }

        private static TimeTag[] shiftingTimeTag(IEnumerable<TimeTag> timeTags, int offset)
            => timeTags.Select(t => TimeTagUtils.ShiftingTimeTag(t, offset)).ToArray();

        private static T[] shiftingTextTag<T>(IEnumerable<T> textTags, string lyric, int offset) where T : ITextTag, new()
            => textTags.Select(t =>
            {
                (int startIndex, int endIndex) = TextTagUtils.GetShiftingIndex(t, lyric, offset);
                return new T
                {
                    Text = t.Text,
                    StartIndex = startIndex,
                    EndIndex = endIndex
                };
            }).ToArray();

        #endregion

        #region Time tags

        public static bool HasTimedTimeTags(IEnumerable<Lyric> lyrics)
            => lyrics.Any(LyricUtils.HasTimedTimeTags);

        #endregion

        #region Lock

        public static Lyric[] FindUnlockLyrics(IEnumerable<Lyric> lyrics)
            => lyrics.Where(x => x.Lock == LockState.None).ToArray();

        #endregion
    }
}
