// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct RecordingTimeTagCaretPosition : IIndexCaretPosition, IComparable<RecordingTimeTagCaretPosition>
{
    public RecordingTimeTagCaretPosition(Lyric lyric, TimeTag timeTag)
    {
        Lyric = lyric;
        TimeTag = timeTag;
    }

    public Lyric Lyric { get; }

    public TimeTag TimeTag { get; }

    public int CompareTo(RecordingTimeTagCaretPosition other)
    {
        if (Lyric != other.Lyric)
            throw new InvalidOperationException();

        return TimeTag.Index.CompareTo(other.TimeTag.Index);
    }

    public int CompareTo(IIndexCaretPosition? other)
    {
        if (other is not RecordingTimeTagCaretPosition recordingTagCaretPosition)
            throw new InvalidOperationException();

        return CompareTo(recordingTagCaretPosition);
    }

    public int GetTotalTimeTags()
    {
        return Lyric.TimeTags.Count;
    }

    public int GetCurrentTimeTagIndex()
    {
        return Lyric.TimeTags.IndexOf(TimeTag);
    }

    public int GetPaddingTextIndex()
    {
        var currentTimeTag = TimeTag;
        return Lyric.TimeTags.SkipWhile(x => x != currentTimeTag).Skip(1).TakeWhile(x => x.Index == currentTimeTag.Index).Count();
    }

    public Tuple<int, int> GetLyricCharRange()
    {
        return GetLyricCharRange(TimeTag);
    }

    public Tuple<int, int> GetLyricCharRange(TimeTag timeTag)
    {
        if (!Lyric.TimeTags.Contains(timeTag))
            throw new InvalidOperationException();

        switch (timeTag.Index.State)
        {
            case TextIndex.IndexState.Start:
            {
                var nextTimeTag = Lyric.TimeTags.GetNextMatch(timeTag, x => x.Index != timeTag.Index);

                int startGapIndex = TextIndexUtils.ToGapIndex(timeTag.Index);
                int endGapIndex = TextIndexUtils.ToGapIndex(nextTimeTag?.Index ?? timeTag.Index);

                return new Tuple<int, int>(startGapIndex, endGapIndex - 1);
            }

            case TextIndex.IndexState.End:
            {
                var previousTimeTag = Lyric.TimeTags.GetPreviousMatch(timeTag, x => x.Index != timeTag.Index);

                int startGapIndex = TextIndexUtils.ToGapIndex(previousTimeTag?.Index ?? timeTag.Index);
                int endGapIndex = TextIndexUtils.ToGapIndex(timeTag.Index);

                return new Tuple<int, int>(startGapIndex, endGapIndex - 1);
            }

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
