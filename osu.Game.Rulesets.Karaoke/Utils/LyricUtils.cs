// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LyricUtils
    {
        #region progessing

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
            lyric.TimeTags = processTimeTags(lyric.TimeTags, position, count);

            // deal with text
            var newLyric = lyric.Text.Substring(0, position) + lyric.Text[(position + count)..];
            lyric.Text = newLyric;

            static T[] processTags<T>(T[] tags, int position, int count) where T : class, ITextTag
            {
                if (tags == null)
                    return null;

                // shifting index.
                foreach (var tag in tags)
                {
                    if (tag.StartIndex > position + count)
                    {
                        tag.StartIndex -= count;
                        tag.EndIndex -= count;
                    }
                    else if (tag.StartIndex > position)
                    {
                        tag.StartIndex = position;
                        tag.EndIndex -= count;
                    }
                    else if (tag.EndIndex > position)
                    {
                        tag.EndIndex = Math.Max(position, tag.EndIndex - count);
                    }
                }

                // if end index less or equal than start index, means this tag has been deleted.
                return tags.Where(x => x.StartIndex < x.EndIndex).ToArray();
            }

            static TimeTag[] processTimeTags(TimeTag[] timeTags, int position, int count)
            {
                var endPosition = position + count;
                return timeTags?.Where(x => !(x.Index.Index >= position && x.Index.Index < endPosition))
                               .Select(t => t.Index.Index > position ? TimeTagUtils.ShiftingTimeTag(t, -count) : t)
                               .ToArray();
            }
        }

        public static void AddText(Lyric lyric, int position, string text)
        {
            if (lyric == null)
                throw new ArgumentNullException($"{nameof(lyric)} cannot be null.");

            // make position is at the range.
            position = Math.Min(Math.Max(0, position), text.Length);

            var shiftingLength = text?.Length ?? 0;
            if (shiftingLength == 0)
                return;

            // deal with ruby and romaji with shifting.
            lyric.RubyTags = processTags(lyric.RubyTags, position, shiftingLength);
            lyric.RomajiTags = processTags(lyric.RomajiTags, position, shiftingLength);
            lyric.TimeTags = processTimeTags(lyric.TimeTags, position, shiftingLength);

            // deal with text
            var newLyric = lyric.Text?.Substring(0, position) + text + lyric.Text?[position..];
            lyric.Text = newLyric;

            static T[] processTags<T>(T[] tags, int position, int shiftingLength) where T : ITextTag
            {
                return tags?.Select(x =>
                           {
                               if (x.StartIndex >= position)
                                   x.StartIndex += shiftingLength;
                               if (x.EndIndex > position)
                                   x.EndIndex += shiftingLength;
                               return x;
                           })
                           .ToArray();
            }

            static TimeTag[] processTimeTags(TimeTag[] timeTags, int startPosition, int shifting)
                => timeTags?.Select(t => t.Index.Index >= startPosition ? TimeTagUtils.ShiftingTimeTag(t, shifting) : t).ToArray();
        }

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
                int endIndex = TextIndexUtils.ToStringIndex(key);

                var text = lyric.Text[startIndex..endIndex];
                var ruby = lyric.RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault()?.Text;

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

        #region Time tag

        public static bool HasTimedTimeTags(Lyric lyric)
        {
            return lyric?.TimeTags?.Any(x => x.Time.HasValue) ?? false;
        }

        public static string GetTimeTagIndexDisplayText(Lyric lyric, TextIndex index)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            var text = lyric.Text;
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            // not showing text if index out of range.
            if (index.Index < 0 || index.Index >= text.Length)
                return "-";

            var timeTags = lyric.TimeTags;

            switch (index.State)
            {
                case TextIndex.IndexState.Start:
                {
                    var nextTimeTag = timeTags.FirstOrDefault(x => x.Index > index);
                    var startIndex = index.Index;
                    var endIndex = TextIndexUtils.ToStringIndex(nextTimeTag?.Index ?? new TextIndex(text.Length));
                    return $"{text.Substring(startIndex, endIndex - startIndex)}-";
                }

                case TextIndex.IndexState.End:
                {
                    var previousTimeTag = timeTags.Reverse().FirstOrDefault(x => x.Index < index);
                    var startIndex = previousTimeTag?.Index.Index ?? 0;
                    var endIndex = index.Index + 1;
                    return $"-{text.Substring(startIndex, endIndex - startIndex)}";
                }

                default:
                    throw new IndexOutOfRangeException(nameof(index.State));
            }
        }

        public static string GetTimeTagDisplayText(Lyric lyric, TimeTag timeTag)
        {
            if (timeTag == null)
                throw new ArgumentNullException(nameof(timeTag));

            return GetTimeTagIndexDisplayText(lyric, timeTag.Index);
        }

        public static string GetTimeTagDisplayRubyText(Lyric lyric, TimeTag timeTag)
        {
            if (timeTag == null)
                throw new ArgumentNullException(nameof(timeTag));

            // should check has ruby in target lyric with target index.
            var matchRuby = lyric?.RubyTags.Where(x =>
            {
                var stringIndex = TextIndexUtils.ToStringIndex(timeTag.Index);

                switch (timeTag.Index.State)
                {
                    case TextIndex.IndexState.Start:
                        return x.StartIndex <= stringIndex && x.EndIndex > stringIndex;

                    case TextIndex.IndexState.End:
                        return x.StartIndex < stringIndex && x.EndIndex >= stringIndex;

                    default:
                        throw new IndexOutOfRangeException(nameof(timeTag.Index.State));
                }
            }).FirstOrDefault();

            if (matchRuby == null || string.IsNullOrEmpty(matchRuby.Text))
                return GetTimeTagDisplayText(lyric, timeTag);

            // get all the rubies with same index.
            var timeTagsWithSameIndex
                = lyric.TimeTags.Where(x => x.Index.Index >= matchRuby.StartIndex && x.Index.Index <= matchRuby.EndIndex);
            var index = timeTagsWithSameIndex.IndexOf(timeTag);

            // get ruby text and should notice exceed case if time-tag is more than ruby text.
            var text = matchRuby.Text;
            var subtext = text.Substring(Math.Min(text.Length - 1, index), 1);

            // return substring with format.
            switch (timeTag.Index.State)
            {
                case TextIndex.IndexState.Start:
                {
                    return $"({subtext})-";
                }

                case TextIndex.IndexState.End:
                {
                    return $"-({subtext})";
                }

                default:
                    throw new IndexOutOfRangeException(nameof(timeTag.Index.State));
            }
        }

        #endregion

        #region Ruby/romaji tag

        public static bool AbleToInsertTextTagAtIndex(Lyric lyric, int index)
            => index >= 0 && index <= (lyric.Text?.Length ?? 0);

        public static bool RemoveTextTag<T>(Lyric lyric, T textTag) where T : ITextTag
        {
            switch (textTag)
            {
                case RubyTag rubyTag:
                    var containRuby = lyric.RubyTags?.Contains(rubyTag) ?? false;
                    lyric.RubyTags = lyric.RubyTags?.Where(x => x != rubyTag).ToArray();
                    return containRuby;

                case RomajiTag romajiTag:
                    var containRomaji = lyric.RomajiTags?.Contains(romajiTag) ?? false;
                    lyric.RomajiTags = lyric.RomajiTags?.Where(x => x != romajiTag).ToArray();
                    return containRomaji;

                default:
                    throw new InvalidCastException(nameof(textTag));
            }
        }

        #endregion

        #region Time display

        public static string LyricTimeFormattedString(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            var startTime = lyric.StartTime.ToEditorFormattedString();
            var endTime = lyric.EndTime.ToEditorFormattedString();
            return $"{startTime} - {endTime}";
        }

        public static string TimeTagTimeFormattedString(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            var availableTimeTags = lyric.TimeTags?.Where(x => x.Time != null);
            var minTimeTag = availableTimeTags?.OrderBy(x => x.Time).FirstOrDefault();
            var maxTimeTag = availableTimeTags?.OrderByDescending(x => x.Time).FirstOrDefault();

            var startTime = TimeTagUtils.FormattedString(minTimeTag);
            var endTime = TimeTagUtils.FormattedString(maxTimeTag);
            return $"{startTime} - {endTime}";
        }

        #endregion

        #region Singer

        public static bool AddSinger(Lyric lyric, Singer singer)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            if (singer == null)
                throw new ArgumentNullException(nameof(singer));

            if (singer.ID <= 0)
                throw new ArgumentOutOfRangeException(nameof(singer.ID));

            // do nothing if already contains singer index.
            if (ContainsSinger(lyric, singer))
                return false;

            var newSingerList = lyric.Singers?.ToList() ?? new List<int>();
            newSingerList.Add(singer.ID);
            lyric.Singers = newSingerList.ToArray();

            return true;
        }

        public static bool RemoveSinger(Lyric lyric, Singer singer)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            if (singer == null)
                throw new ArgumentNullException(nameof(singer));

            if (singer.ID <= 0)
                throw new ArgumentOutOfRangeException(nameof(singer.ID));

            // do nothing if not contains singer index.
            if (!ContainsSinger(lyric, singer))
                return false;

            var newSingerIds = lyric.Singers.Where(x => x != singer.ID).ToArray();
            lyric.Singers = newSingerIds;

            return true;
        }

        public static bool ContainsSinger(Lyric lyric, Singer singer)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            if (singer == null)
                throw new ArgumentNullException(nameof(singer));

            return lyric.Singers?.Contains(singer.ID) ?? false;
        }

        public static bool OnlyContainsSingers(Lyric lyric, List<Singer> singers)
        {
            if (singers == null)
                throw new ArgumentNullException(nameof(singers));

            var singerIds = singers.Select(x => x.ID);
            return lyric.Singers?.All(x => singerIds.Contains(x)) ?? true;
        }

        #endregion

        #region Check

        /// <summary>
        /// Check start time is larger than end time.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public static bool CheckIsTimeOverlapping(Lyric lyric)
        {
            return lyric.StartTime > lyric.EndTime;
        }

        /// <summary>
        /// Start time should be smaller than any time-tag.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public static bool CheckIsStartTimeInvalid(Lyric lyric)
        {
            if (lyric.TimeTags == null || lyric.TimeTags.Length == 0)
                return false;

            return lyric.StartTime > TimeTagsUtils.GetStartTime(lyric.TimeTags);
        }

        /// <summary>
        /// End time should be larger than any time-tag.
        /// </summary>
        /// <param name="lyric"></param>
        /// <returns></returns>
        public static bool CheckIsEndTimeInvalid(Lyric lyric)
        {
            if (lyric.TimeTags == null || lyric.TimeTags.Length == 0)
                return false;

            return lyric.EndTime < TimeTagsUtils.GetEndTime(lyric.TimeTags);
        }

        #endregion
    }
}
