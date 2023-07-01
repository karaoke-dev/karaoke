// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Objects.Utils;

public static class LyricUtils
{
    #region progessing

    public static void RemoveText(Lyric lyric, int charGap, int count = 1)
    {
        ArgumentNullException.ThrowIfNull(lyric);

        int textLength = lyric.Text.Length;
        if (textLength == 0)
            return;

        if (charGap < 0 || charGap > textLength)
            throw new ArgumentOutOfRangeException(nameof(charGap));

        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(charGap));

        if (charGap + count >= textLength)
            count = textLength - charGap;

        // deal with ruby and romaji, might remove and shifting.
        lyric.RubyTags = processTags(lyric.RubyTags, charGap, count);
        lyric.RomajiTags = processTags(lyric.RomajiTags, charGap, count);
        lyric.TimeTags = processTimeTags(lyric.TimeTags, charGap, count);

        // deal with text
        string newLyric = lyric.Text[..charGap] + lyric.Text[(charGap + count)..];
        lyric.Text = newLyric;

        static IList<T> processTags<T>(IList<T> tags, int charGap, int count) where T : class, ITextTag
        {
            // shifting index.
            foreach (var tag in tags)
            {
                if (tag.StartIndex > charGap + count)
                {
                    tag.StartIndex -= count;
                    tag.EndIndex -= count;
                }
                else if (tag.StartIndex > charGap)
                {
                    tag.StartIndex = charGap;
                    tag.EndIndex -= count;
                }
                else if (tag.EndIndex >= charGap)
                {
                    tag.EndIndex = Math.Max(charGap - 1, tag.EndIndex - count);
                }
            }

            // if end index less or equal than start index, means this tag has been deleted.
            return tags.Where(x => x.StartIndex <= x.EndIndex).ToArray();
        }

        static IList<TimeTag> processTimeTags(IEnumerable<TimeTag> timeTags, int charGap, int count)
        {
            int endCharGap = charGap + count;
            return timeTags.Where(x => !(x.Index.Index >= charGap && x.Index.Index < endCharGap))
                           .Select(t => t.Index.Index > charGap ? TimeTagUtils.ShiftingTimeTag(t, -count) : t)
                           .ToArray();
        }
    }

    public static void AddText(Lyric lyric, int charGap, string text)
    {
        ArgumentNullException.ThrowIfNull(lyric);

        // make position is at the range.
        string lyricText = lyric.Text;
        int lyricTextLength = lyricText.Length;
        charGap = Math.Clamp(charGap, 0, lyricTextLength);

        int offset = text.Length;
        if (offset == 0)
            return;

        // deal with ruby and romaji with shifting.
        lyric.RubyTags = processTags(lyric.RubyTags, charGap, offset);
        lyric.RomajiTags = processTags(lyric.RomajiTags, charGap, offset);
        lyric.TimeTags = processTimeTags(lyric.TimeTags, charGap, offset);

        // deal with text
        string newLyricText = lyricText[..charGap] + text + lyricText[charGap..];
        lyric.Text = newLyricText;

        static T[] processTags<T>(IEnumerable<T> tags, int charGap, int offset) where T : ITextTag =>
            tags.Select(x =>
                {
                    if (x.StartIndex >= charGap)
                        x.StartIndex += offset;
                    if (x.EndIndex >= charGap)
                        x.EndIndex += offset;
                    return x;
                })
                .ToArray();

        static TimeTag[] processTimeTags(IEnumerable<TimeTag> timeTags, int charGap, int offset)
            => timeTags.Select(t => t.Index.Index >= charGap ? TimeTagUtils.ShiftingTimeTag(t, offset) : t).ToArray();
    }

    #endregion

    #region Time tag

    public static bool HasTimedTimeTags(Lyric lyric)
        => lyric.TimeTags.Any(x => x.Time.HasValue);

    public static string GetTimeTagIndexDisplayText(Lyric lyric, TextIndex index)
    {
        ArgumentNullException.ThrowIfNull(lyric);

        string text = lyric.Text;
        if (string.IsNullOrEmpty(text))
            throw new ArgumentNullException(nameof(text));

        // not showing text if index out of range.
        if (index.Index < 0 || index.Index >= text.Length)
            return "-";

        var timeTags = lyric.TimeTags;

        return TextIndexUtils.GetValueByState(index, () =>
        {
            var nextTimeTag = timeTags.FirstOrDefault(x => x.Index > index);
            int startIndex = index.Index;
            int endIndex = TextIndexUtils.ToGapIndex(nextTimeTag?.Index ?? new TextIndex(text.Length));
            return $"{text.Substring(startIndex, endIndex - startIndex)}-";
        }, () =>
        {
            var previousTimeTag = timeTags.Reverse().FirstOrDefault(x => x.Index < index);
            int startIndex = previousTimeTag?.Index.Index ?? 0;
            int endIndex = index.Index + 1;
            return $"-{text.Substring(startIndex, endIndex - startIndex)}";
        });
    }

    public static string GetTimeTagDisplayText(Lyric lyric, TimeTag timeTag)
    {
        ArgumentNullException.ThrowIfNull(timeTag);

        return GetTimeTagIndexDisplayText(lyric, timeTag.Index);
    }

    public static string GetTimeTagDisplayRubyText(Lyric lyric, TimeTag timeTag)
    {
        ArgumentNullException.ThrowIfNull(timeTag);

        var state = timeTag.Index.State;

        // should check has ruby in target lyric with target index.
        var matchRuby = lyric.RubyTags.Where(x =>
        {
            int charIndex = TextIndexUtils.ToCharIndex(timeTag.Index);
            return TextIndexUtils.GetValueByState(state,
                () => x.StartIndex <= charIndex && x.EndIndex >= charIndex,
                () => x.StartIndex <= charIndex && x.EndIndex >= charIndex);
        }).FirstOrDefault();

        if (matchRuby == null || string.IsNullOrEmpty(matchRuby.Text))
            return GetTimeTagDisplayText(lyric, timeTag);

        // get all the rubies with same index.
        var timeTagsWithSameIndex = lyric.TimeTags.Where(x =>
        {
            var startTextIndex = new TextIndex(matchRuby.StartIndex);
            var endTextIndex = new TextIndex(matchRuby.EndIndex, TextIndex.IndexState.End);

            return x.Index >= startTextIndex && x.Index <= endTextIndex;
        }).ToList();

        // get ruby text and should notice exceed case if time-tag is more than ruby text.
        int index = timeTagsWithSameIndex.IndexOf(timeTag);
        string text = matchRuby.Text;
        string subtext = timeTagsWithSameIndex.Count == 1 ? text : text.Substring(Math.Min(text.Length - 1, index), 1);

        // return substring with format.
        return TextIndexUtils.GetValueByState(state, $"({subtext})-", $"-({subtext})");
    }

    #endregion

    #region Ruby/romaji tag

    public static bool AbleToInsertTextTagAtIndex(Lyric lyric, int index)
        => index >= 0 && index <= lyric.Text.Length;

    #endregion

    #region Time display

    public static string LyricTimeFormattedString(Lyric lyric)
    {
        ArgumentNullException.ThrowIfNull(lyric);

        string startTime = lyric.StartTime.ToEditorFormattedString();
        string endTime = lyric.EndTime.ToEditorFormattedString();
        return $"{startTime} - {endTime}";
    }

    public static string TimeTagTimeFormattedString(Lyric lyric)
    {
        ArgumentNullException.ThrowIfNull(lyric);

        var availableTimeTags = lyric.TimeTags.Where(x => x.Time != null).ToArray();
        var minTimeTag = availableTimeTags.MinBy(x => x.Time);
        var maxTimeTag = availableTimeTags.MaxBy(x => x.Time);

        string startTime = TimeTagUtils.FormattedString(minTimeTag ?? new TimeTag(new TextIndex()));
        string endTime = TimeTagUtils.FormattedString(maxTimeTag ?? new TimeTag(new TextIndex()));
        return $"{startTime} - {endTime}";
    }

    #endregion

    #region Singer

    public static bool ContainsSinger(Lyric lyric, Singer singer)
    {
        ArgumentNullException.ThrowIfNull(lyric);
        ArgumentNullException.ThrowIfNull(singer);

        return lyric.SingerIds.Contains(singer.ID);
    }

    public static bool OnlyContainsSingers(Lyric lyric, List<Singer> singers)
    {
        ArgumentNullException.ThrowIfNull(singers);

        var singerIds = singers.Select(x => x.ID);
        return lyric.SingerIds.All(x => singerIds.Contains(x));
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
        if (!lyric.TimeTags.Any())
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
        if (!lyric.TimeTags.Any())
            return false;

        return lyric.EndTime < TimeTagsUtils.GetEndTime(lyric.TimeTags);
    }

    #endregion
}
