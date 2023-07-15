// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Utils;

public static class TextIndexUtils
{
    public static int ToGapIndex(TextIndex index)
        => GetValueByState(index, index.Index, index.Index + 1);

    public static int ToCharIndex(TextIndex index)
        => index.Index;

    public static TextIndex FromStringIndex(int index, bool end)
    {
        return end ? new TextIndex(index - 1, TextIndex.IndexState.End) : new TextIndex(index);
    }

    public static TextIndex.IndexState ReverseState(TextIndex.IndexState state)
    {
        return GetValueByState(state, TextIndex.IndexState.End, TextIndex.IndexState.Start);
    }

    public static TextIndex GetPreviousIndex(TextIndex originIndex)
    {
        int previousIndex = ToGapIndex(originIndex) - 1;
        var previousState = ReverseState(originIndex.State);
        return new TextIndex(previousIndex, previousState);
    }

    public static TextIndex GetNextIndex(TextIndex originIndex)
    {
        int nextIndex = ToGapIndex(originIndex);
        var nextState = ReverseState(originIndex.State);
        return new TextIndex(nextIndex, nextState);
    }

    public static TextIndex ShiftingIndex(TextIndex originIndex, int offset)
        => new(originIndex.Index + offset, originIndex.State);

    public static bool OutOfRange(TextIndex index, string lyric)
    {
        if (string.IsNullOrEmpty(lyric))
            return true;

        return index.Index < 0 || index.Index >= lyric.Length;
    }

    public static T GetValueByState<T>(TextIndex index, T startValue, T endValue) =>
        GetValueByState(index.State, startValue, endValue);

    public static T GetValueByState<T>(TextIndex.IndexState state, T startValue, T endValue) =>
        state switch
        {
            TextIndex.IndexState.Start => startValue,
            TextIndex.IndexState.End => endValue,
            _ => throw new ArgumentOutOfRangeException(nameof(state)),
        };

    public static T GetValueByState<T>(TextIndex index, Func<T> startValue, Func<T> endValue) =>
        GetValueByState(index.State, startValue, endValue);

    public static T GetValueByState<T>(TextIndex.IndexState state, Func<T> startValue, Func<T> endValue) =>
        state switch
        {
            TextIndex.IndexState.Start => startValue(),
            TextIndex.IndexState.End => endValue(),
            _ => throw new ArgumentOutOfRangeException(nameof(state)),
        };

    /// <summary>
    /// Display string with position format
    /// </summary>
    /// <example>
    /// 3
    /// 4(end)
    /// </example>
    /// <param name="textIndex"></param>
    /// <returns></returns>
    public static string PositionFormattedString(TextIndex textIndex)
    {
        int index = textIndex.Index;
        string state = GetValueByState(textIndex, string.Empty, "(end)");
        return $"{index}{state}";
    }
}
