﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Objects.Utils;

public static class LyricsUtils
{
    #region processing

    public static Tuple<Lyric, Lyric> SplitLyric(Lyric lyric, int splitIndex)
    {
        ArgumentNullException.ThrowIfNull(lyric);

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
        firstLyric.RubyTags = lyric.RubyTags.Where(x => x.StartIndex < splitIndex && x.EndIndex < splitIndex).ToArray();

        // todo : should implement time and duration
        string secondLyricText = lyric.Text[splitIndex..];
        var secondLyric = lyric.DeepClone();
        secondLyric.Text = secondLyricText;
        secondLyric.TimeTags = shiftingTimeTag(secondTimeTag.ToArray(), -splitIndex);
        secondLyric.RubyTags = shiftingRubyTag(lyric.RubyTags.Where(x => x.StartIndex >= splitIndex && x.EndIndex >= splitIndex).ToArray(), secondLyricText, -splitIndex);

        return new Tuple<Lyric, Lyric>(firstLyric, secondLyric);
    }

    public static Lyric CombineLyric(Lyric firstLyric, Lyric secondLyric)
    {
        ArgumentNullException.ThrowIfNull(firstLyric);
        ArgumentNullException.ThrowIfNull(secondLyric);

        int offsetIndexForSecondLyric = firstLyric.Text.Length;
        string lyricText = firstLyric.Text + secondLyric.Text;

        var timeTags = new List<TimeTag>();
        timeTags.AddRange(firstLyric.TimeTags);
        timeTags.AddRange(shiftingTimeTag(secondLyric.TimeTags, offsetIndexForSecondLyric));

        var rubyTags = new List<RubyTag>();
        rubyTags.AddRange(firstLyric.RubyTags);
        rubyTags.AddRange(shiftingRubyTag(secondLyric.RubyTags, lyricText, offsetIndexForSecondLyric));

        var singers = new List<ElementId>();
        singers.AddRange(firstLyric.SingerIds);
        singers.AddRange(secondLyric.SingerIds);

        bool sameLanguage = EqualityComparer<CultureInfo?>.Default.Equals(firstLyric.Language, secondLyric.Language);
        var language = sameLanguage ? firstLyric.Language : null;

        return new Lyric
        {
            Text = lyricText,
            TimeTags = timeTags.ToArray(),
            RubyTags = rubyTags.ToArray(),
            SingerIds = singers.Distinct().ToArray(),
            Language = language,
        };
    }

    private static TimeTag[] shiftingTimeTag(IEnumerable<TimeTag> timeTags, int offset)
        => timeTags.Select(t => TimeTagUtils.ShiftingTimeTag(t, offset)).ToArray();

    private static RubyTag[] shiftingRubyTag(IEnumerable<RubyTag> rubyTags, string lyric, int offset)
        => rubyTags.Select(t =>
        {
            (int startIndex, int endIndex) = RubyTagUtils.GetShiftingIndex(t, lyric, offset);
            return new RubyTag
            {
                Text = t.Text,
                StartIndex = startIndex,
                EndIndex = endIndex,
            };
        }).ToArray();

    #endregion

    #region Time tags

    public static bool HasTimedTimeTags(IEnumerable<Lyric> lyrics)
        => lyrics.Any(LyricUtils.HasTimedTimeTags);

    #endregion
}
