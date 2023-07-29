// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Algorithm for only accept navigate to the target lyric by mouse click.
/// </summary>
public class ClickingCaretPositionAlgorithm : CaretPositionAlgorithm<ClickingCaretPosition>
{
    public ClickingCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override bool PositionMovable(ClickingCaretPosition position)
    {
        return true;
    }

    protected override ClickingCaretPosition? MoveToPreviousLyric(ClickingCaretPosition currentPosition)
    {
        return null;
    }

    protected override ClickingCaretPosition? MoveToNextLyric(ClickingCaretPosition currentPosition)
    {
        return null;
    }

    protected override ClickingCaretPosition? MoveToFirstLyric()
    {
        return null;
    }

    protected override ClickingCaretPosition? MoveToLastLyric()
    {
        return null;
    }

    protected override ClickingCaretPosition? MoveToTargetLyric(Lyric lyric) => new(lyric);
}
