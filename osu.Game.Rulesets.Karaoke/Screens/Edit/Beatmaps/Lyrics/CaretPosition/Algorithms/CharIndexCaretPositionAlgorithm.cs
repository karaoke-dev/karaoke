// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

/// <summary>
/// Base class for those algorithms which use char index as index.
/// </summary>
/// <typeparam name="TCaretPosition"></typeparam>
public abstract class CharIndexCaretPositionAlgorithm<TCaretPosition> : IndexCaretPositionAlgorithm<TCaretPosition, int>
    where TCaretPosition : struct, ICharIndexCaretPosition
{
    protected CharIndexCaretPositionAlgorithm(Lyric[] lyrics)
        : base(lyrics)
    {
    }

    protected sealed override bool PositionMovable(TCaretPosition position)
    {
        return indexInTextRange(position.CharIndex, position.Lyric);
    }

    protected sealed override TCaretPosition? MoveToPreviousLyric(TCaretPosition currentPosition)
    {
        var lyric = Lyrics.GetPreviousMatch(currentPosition.Lyric, lyricMovable);
        if (lyric == null)
            return null;

        int minIndex = getMinIndex(lyric.Text);
        int maxIndex = getMaxIndex(lyric.Text);
        if (maxIndex < minIndex)
            return null;

        int index = Math.Clamp(currentPosition.CharIndex, minIndex, maxIndex);

        return CreateCaretPosition(lyric, index);
    }

    protected sealed override TCaretPosition? MoveToNextLyric(TCaretPosition currentPosition)
    {
        var lyric = Lyrics.GetNextMatch(currentPosition.Lyric, lyricMovable);
        if (lyric == null)
            return null;

        int index = Math.Clamp(currentPosition.CharIndex, getMinIndex(lyric.Text), getMaxIndex(lyric.Text));

        return CreateCaretPosition(lyric, index);
    }

    protected sealed override TCaretPosition? MoveToFirstLyric()
    {
        var lyric = Lyrics.FirstOrDefault(lyricMovable);
        if (lyric == null)
            return null;

        return CreateCaretPosition(lyric, getMinIndex(lyric.Text));
    }

    protected sealed override TCaretPosition? MoveToLastLyric()
    {
        var lyric = Lyrics.LastOrDefault(lyricMovable);
        if (lyric == null)
            return null;

        return CreateCaretPosition(lyric, getMaxIndex(lyric.Text));
    }

    protected sealed override TCaretPosition? MoveToTargetLyric(Lyric lyric)
        => CreateCaretPosition(lyric, getMinIndex(lyric.Text));

    protected sealed override TCaretPosition? MoveToPreviousIndex(TCaretPosition currentPosition)
    {
        // get previous caret and make a check is need to change line.
        var lyric = currentPosition.Lyric;
        int previousIndex = currentPosition.CharIndex - 1;

        if (!indexInTextRange(previousIndex, lyric))
            return null;

        return CreateCaretPosition(lyric, previousIndex);
    }

    protected sealed override TCaretPosition? MoveToNextIndex(TCaretPosition currentPosition)
    {
        // get next caret and make a check is need to change line.
        var lyric = currentPosition.Lyric;
        int nextIndex = currentPosition.CharIndex + 1;

        if (!indexInTextRange(nextIndex, lyric))
            return null;

        return CreateCaretPosition(lyric, nextIndex);
    }

    protected sealed override TCaretPosition? MoveToFirstIndex(Lyric lyric)
    {
        int index = getMinIndex(lyric.Text);

        return CreateCaretPosition(lyric, index);
    }

    protected sealed override TCaretPosition? MoveToLastIndex(Lyric lyric)
    {
        int index = getMaxIndex(lyric.Text);

        return CreateCaretPosition(lyric, index);
    }

    private bool lyricMovable(Lyric lyric)
    {
        int minIndex = getMinIndex(lyric.Text);
        return indexInTextRange(minIndex, lyric);
    }

    private static bool indexInTextRange(int index, Lyric lyric)
    {
        string text = lyric.Text;
        return index >= getMinIndex(text) && index <= getMaxIndex(text);
    }

    private static int getMinIndex(string text) => 0;

    private static int getMaxIndex(string text) => text.Length - 1;
}
