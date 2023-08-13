// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

public readonly struct TimeTagCaretPosition : IIndexCaretPosition, IComparable<TimeTagCaretPosition>
{
    public TimeTagCaretPosition(Lyric lyric, TimeTag timeTag)
    {
        Lyric = lyric;
        TimeTag = timeTag;
    }

    public Lyric Lyric { get; }

    public TimeTag TimeTag { get; }

    public int CompareTo(TimeTagCaretPosition other)
    {
        if (Lyric != other.Lyric)
            throw new InvalidOperationException();

        return TimeTag.Index.CompareTo(other.TimeTag.Index);
    }

    public int CompareTo(IIndexCaretPosition? other)
    {
        if (other is not TimeTagCaretPosition timeTagCaretPosition)
            throw new InvalidOperationException();

        return CompareTo(timeTagCaretPosition);
    }
}
