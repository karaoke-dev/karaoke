// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Base class for those algorithms which use char gap as index.
/// </summary>
/// <typeparam name="TCaretPosition"></typeparam>
public abstract class CharGapCaretPositionAlgorithm<TCaretPosition> : IndexCaretPositionAlgorithm<TCaretPosition, int>
    where TCaretPosition : struct, ICharGapCaretPosition
{
    protected CharGapCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected override bool PositionMovable(TCaretPosition position)
    {
        return IndexInTextRange(position.CharGap, position.Lyric);
    }

    protected sealed override TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition)
    {
        var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, lyricMovable);
        if (lyric == null)
            return null;

        int minIndex = GetMinIndex(lyric.Text);
        int maxIndex = GetMaxIndex(lyric.Text);
        if (maxIndex < minIndex)
            return null;

        int index = Math.Clamp(currentPosition.CharGap, minIndex, maxIndex);

        return CreateCaretPosition(lyric, index);
    }

    protected sealed override TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition)
    {
        var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, lyricMovable);
        if (lyric == null)
            return null;

        int index = Math.Clamp(currentPosition.CharGap, GetMinIndex(lyric.Text), GetMaxIndex(lyric.Text));

        return CreateCaretPosition(lyric, index);
    }

    protected sealed override TCaretPosition? MoveToFirstLyric()
    {
        var lyric = Lyrics.FirstOrDefault(lyricMovable);
        if (lyric == null)
            return null;

        return CreateCaretPosition(lyric, GetMinIndex(lyric.Text));
    }

    protected sealed override TCaretPosition? MoveToLastLyric()
    {
        var lyric = Lyrics.LastOrDefault(lyricMovable);
        if (lyric == null)
            return null;

        return CreateCaretPosition(lyric, GetMaxIndex(lyric.Text));
    }

    protected sealed override TCaretPosition? MoveToTargetLyric(Lyric lyric)
        => CreateCaretPosition(lyric, GetMinIndex(lyric.Text), CaretGenerateType.TargetLyric);

    protected sealed override TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition)
    {
        // get previous caret and make a check is need to change line.
        var lyric = currentPosition.Lyric;
        int previousIndex = currentPosition.CharGap - 1;

        if (!IndexInTextRange(previousIndex, lyric))
            return null;

        return CreateCaretPosition(lyric, previousIndex);
    }

    protected sealed override TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition)
    {
        // get next caret and make a check is need to change line.
        var lyric = currentPosition.Lyric;
        int nextIndex = currentPosition.CharGap + 1;

        if (!IndexInTextRange(nextIndex, lyric))
            return null;

        return CreateCaretPosition(lyric, nextIndex);
    }

    protected sealed override TCaretPosition? MoveToFirstIndex(Lyric lyric)
    {
        int index = GetMinIndex(lyric.Text);

        return CreateCaretPosition(lyric, index);
    }

    protected sealed override TCaretPosition? MoveToLastIndex(Lyric lyric)
    {
        int index = GetMaxIndex(lyric.Text);

        return CreateCaretPosition(lyric, index);
    }

    private bool lyricMovable(Lyric lyric)
    {
        int minIndex = GetMinIndex(lyric.Text);
        return IndexInTextRange(minIndex, lyric);
    }

    protected bool IndexInTextRange(int index, Lyric lyric)
    {
        string text = lyric.Text;
        return index >= GetMinIndex(text) && index <= GetMaxIndex(text);
    }

    protected abstract int GetMinIndex(string text);

    protected abstract int GetMaxIndex(string text);
}
