// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for navigate to the <see cref="TimeTag"/> position inside the <see cref="Lyric"/>.
/// </summary>
public class TimeTagCaretPositionAlgorithm : IndexCaretPositionAlgorithm<TimeTagCaretPosition, TimeTag>
{
    public MovingTimeTagCaretMode Mode { get; set; }

    public TimeTagCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override void PreValidate(TimeTagCaretPosition input)
    {
        var timeTag = input.TimeTag;
        var lyric = input.Lyric;

        // should only check if the time-tag is in the lyric because previous time-tag position might not match to the mode.
        Debug.Assert(lyric.TimeTags.Contains(timeTag));
    }

    protected override bool PositionMovable(TimeTagCaretPosition position)
    {
        var lyric = position.Lyric;
        var timeTag = position.TimeTag;

        return lyric.TimeTags.Contains(timeTag)
               && timeTagMovable(timeTag);
    }

    protected override TimeTagCaretPosition? MoveToPreviousLyric(TimeTagCaretPosition currentPosition)
    {
        var currentTimeTag = currentPosition.TimeTag;

        var currentLyric = currentPosition.Lyric;
        var previousLyric = Lyrics.GetPreviousMatch(currentLyric, l => l.TimeTags.Any(timeTagMovable));
        if (previousLyric == null)
            return null;

        var timeTags = previousLyric.TimeTags.Where(timeTagMovable).ToArray();
        var upTimeTag = timeTags.FirstOrDefault(x => x.Index >= currentTimeTag.Index)
                        ?? timeTags.LastOrDefault();

        if (upTimeTag == null)
            return null;

        return CreateCaretPosition(previousLyric, upTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToNextLyric(TimeTagCaretPosition currentPosition)
    {
        var currentTimeTag = currentPosition.TimeTag;

        var currentLyric = currentPosition.Lyric;
        var nextLyric = Lyrics.GetNextMatch(currentLyric, l => l.TimeTags.Any(timeTagMovable));
        if (nextLyric == null)
            return null;

        var timeTags = nextLyric.TimeTags.Where(timeTagMovable).ToArray();
        var downTimeTag = timeTags.FirstOrDefault(x => x.Index >= currentTimeTag.Index)
                          ?? timeTags.LastOrDefault();

        if (downTimeTag == null)
            return null;

        return CreateCaretPosition(nextLyric, downTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToFirstLyric()
    {
        var firstLyric = Lyrics.FirstOrDefault(x => x.TimeTags.Any(timeTagMovable));
        var firstTimeTag = firstLyric?.TimeTags.FirstOrDefault(timeTagMovable);
        if (firstLyric == null || firstTimeTag == null)
            return null;

        return CreateCaretPosition(firstLyric, firstTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToLastLyric()
    {
        var lastLyric = Lyrics.LastOrDefault(x => x.TimeTags.Any(timeTagMovable));
        var lastTimeTag = lastLyric?.TimeTags.LastOrDefault(timeTagMovable);
        if (lastLyric == null || lastTimeTag == null)
            return null;

        return CreateCaretPosition(lastLyric, lastTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToTargetLyric(Lyric lyric)
    {
        var targetTimeTag = lyric.TimeTags.FirstOrDefault(timeTagMovable);
        if (targetTimeTag == null)
            return null;

        return CreateCaretPosition(lyric, targetTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToPreviousIndex(TimeTagCaretPosition currentPosition)
    {
        var lyric = currentPosition.Lyric;
        var timeTags = lyric.TimeTags;
        var previousTimeTag = timeTags.GetPreviousMatch(currentPosition.TimeTag, timeTagMovable);
        if (previousTimeTag == null)
            return null;

        return CreateCaretPosition(lyric, previousTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToNextIndex(TimeTagCaretPosition currentPosition)
    {
        var lyric = currentPosition.Lyric;
        var timeTags = lyric.TimeTags;
        var nextTimeTag = timeTags.GetNextMatch(currentPosition.TimeTag, timeTagMovable);
        if (nextTimeTag == null)
            return null;

        return CreateCaretPosition(lyric, nextTimeTag);
    }

    protected override TimeTagCaretPosition? MoveToFirstIndex(Lyric lyric)
    {
        var firstTimeTag = lyric.TimeTags.FirstOrDefault();
        if (firstTimeTag == null)
            return null;

        var caret = CreateCaretPosition(lyric, firstTimeTag);
        if (!timeTagMovable(firstTimeTag))
            return MoveToNextIndex(caret);

        return caret;
    }

    protected override TimeTagCaretPosition? MoveToLastIndex(Lyric lyric)
    {
        var lastTimeTag = lyric.TimeTags.LastOrDefault();
        if (lastTimeTag == null)
            return null;

        var caret = CreateCaretPosition(lyric, lastTimeTag);
        if (!timeTagMovable(lastTimeTag))
            return MoveToPreviousIndex(caret);

        return caret;
    }

    protected override TimeTagCaretPosition CreateCaretPosition(Lyric lyric, TimeTag index) => new(lyric, index);

    private bool timeTagMovable(TimeTag timeTag)
    {
        return Mode switch
        {
            MovingTimeTagCaretMode.None => true,
            MovingTimeTagCaretMode.OnlyStartTag => timeTag.Index.State == TextIndex.IndexState.Start,
            MovingTimeTagCaretMode.OnlyEndTag => timeTag.Index.State == TextIndex.IndexState.End,
            _ => throw new InvalidOperationException(nameof(MovingTimeTagCaretMode)),
        };
    }
}
