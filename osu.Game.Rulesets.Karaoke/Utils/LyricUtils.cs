// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class LyricUtils
    {
        #region progessing

        public static void RemoveText(Lyric lyric, int position, int count = 1)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            int textLength = lyric.Text.Length;
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
            string newLyric = lyric.Text[..position] + lyric.Text[(position + count)..];
            lyric.Text = newLyric;

            static IList<T> processTags<T>(IList<T> tags, int position, int count) where T : class, ITextTag
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

            static IList<TimeTag> processTimeTags(IEnumerable<TimeTag> timeTags, int position, int count)
            {
                int endPosition = position + count;
                return timeTags?.Where(x => !(x.Index.Index >= position && x.Index.Index < endPosition))
                               .Select(t => t.Index.Index > position ? TimeTagUtils.ShiftingTimeTag(t, -count) : t)
                               .ToArray();
            }
        }

        public static void AddText(Lyric lyric, int position, string text)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            // make position is at the range.
            string lyricText = lyric.Text;
            int lyricTextLength = lyricText?.Length ?? 0;
            position = Math.Clamp(position, 0, lyricTextLength);

            int shiftingLength = text?.Length ?? 0;
            if (shiftingLength == 0)
                return;

            // deal with ruby and romaji with shifting.
            lyric.RubyTags = processTags(lyric.RubyTags, position, shiftingLength);
            lyric.RomajiTags = processTags(lyric.RomajiTags, position, shiftingLength);
            lyric.TimeTags = processTimeTags(lyric.TimeTags, position, shiftingLength);

            // deal with text
            string newLyricText = lyricText?[..position] + text + lyricText?[position..];
            lyric.Text = newLyricText;

            static T[] processTags<T>(IEnumerable<T> tags, int position, int shiftingLength) where T : ITextTag =>
                tags?.Select(x =>
                    {
                        if (x.StartIndex >= position)
                            x.StartIndex += shiftingLength;
                        if (x.EndIndex > position)
                            x.EndIndex += shiftingLength;
                        return x;
                    })
                    .ToArray();

            static TimeTag[] processTimeTags(IEnumerable<TimeTag> timeTags, int startPosition, int offset)
                => timeTags?.Select(t => t.Index.Index >= startPosition ? TimeTagUtils.ShiftingTimeTag(t, offset) : t).ToArray();
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

            string text = lyric.Text;
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
                    int startIndex = index.Index;
                    int endIndex = TextIndexUtils.ToStringIndex(nextTimeTag?.Index ?? new TextIndex(text.Length));
                    return $"{text.Substring(startIndex, endIndex - startIndex)}-";
                }

                case TextIndex.IndexState.End:
                {
                    var previousTimeTag = timeTags.Reverse().FirstOrDefault(x => x.Index < index);
                    int startIndex = previousTimeTag?.Index.Index ?? 0;
                    int endIndex = index.Index + 1;
                    return $"-{text.Substring(startIndex, endIndex - startIndex)}";
                }

                default:
                    throw new InvalidEnumArgumentException(nameof(index.State));
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

            var state = timeTag.Index.State;

            // should check has ruby in target lyric with target index.
            var matchRuby = lyric?.RubyTags.Where(x =>
            {
                int stringIndex = TextIndexUtils.ToStringIndex(timeTag.Index);

                return state switch
                {
                    TextIndex.IndexState.Start => x.StartIndex <= stringIndex && x.EndIndex > stringIndex,
                    TextIndex.IndexState.End => x.StartIndex < stringIndex && x.EndIndex >= stringIndex,
                    _ => throw new InvalidEnumArgumentException(nameof(state))
                };
            }).FirstOrDefault();

            if (matchRuby == null || string.IsNullOrEmpty(matchRuby.Text))
                return GetTimeTagDisplayText(lyric, timeTag);

            // get all the rubies with same index.
            var timeTagsWithSameIndex = lyric.TimeTags.Where(x =>
            {
                if (x.Index.Index < matchRuby.StartIndex || x.Index.Index > matchRuby.EndIndex)
                    return false;

                return x.Index.State != TextIndex.IndexState.Start || x.Index.Index != matchRuby.EndIndex;
            }).ToList();

            // get ruby text and should notice exceed case if time-tag is more than ruby text.
            int index = timeTagsWithSameIndex.IndexOf(timeTag);
            string text = matchRuby.Text;
            string subtext = timeTagsWithSameIndex.Count == 1 ? text : text.Substring(Math.Min(text.Length - 1, index), 1);

            // return substring with format.
            return state switch
            {
                TextIndex.IndexState.Start => $"({subtext})-",
                TextIndex.IndexState.End => $"-({subtext})",
                _ => throw new InvalidEnumArgumentException(nameof(state))
            };
        }

        #endregion

        #region Ruby/romaji tag

        public static bool AbleToInsertTextTagAtIndex(Lyric lyric, int index)
            => index >= 0 && index <= (lyric.Text?.Length ?? 0);

        #endregion

        #region Time display

        public static string LyricTimeFormattedString(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            string startTime = lyric.StartTime.ToEditorFormattedString();
            string endTime = lyric.EndTime.ToEditorFormattedString();
            return $"{startTime} - {endTime}";
        }

        public static string TimeTagTimeFormattedString(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            var availableTimeTags = lyric.TimeTags?.Where(x => x.Time != null).ToArray();
            var minTimeTag = availableTimeTags?.OrderBy(x => x.Time).FirstOrDefault();
            var maxTimeTag = availableTimeTags?.OrderByDescending(x => x.Time).FirstOrDefault();

            string startTime = TimeTagUtils.FormattedString(minTimeTag);
            string endTime = TimeTagUtils.FormattedString(maxTimeTag);
            return $"{startTime} - {endTime}";
        }

        #endregion

        #region Layout

        public static void AssignLayout(Lyric lyric, LyricLayout layout)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            if (layout.ID < 0)
                throw new InvalidOperationException($"{nameof(layout.ID)} cannot be negative");

            lyric.LayoutIndex = layout.ID;
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

            int[] newSingerIds = lyric.Singers.Where(x => x != singer.ID).ToArray();
            lyric.Singers = newSingerIds;

            return true;
        }

        public static bool ClearSinger(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            if (lyric.Singers == null || !lyric.Singers.Any())
                return false;

            lyric.Singers = new List<int>();
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
            if (lyric.TimeTags == null || !lyric.TimeTags.Any())
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
            if (lyric.TimeTags == null || !lyric.TimeTags.Any())
                return false;

            return lyric.EndTime < TimeTagsUtils.GetEndTime(lyric.TimeTags);
        }

        #endregion
    }
}
