// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Objects.Utils;

public static class RubyTagUtils
{
    public static Tuple<int, int> GetFixedIndex(RubyTag rubyTag, string lyric)
        => GetShiftingIndex(rubyTag, lyric, 0);

    public static Tuple<int, int> GetShiftingIndex(RubyTag rubyTag, string lyric, int offset)
    {
        if (string.IsNullOrEmpty(lyric))
            throw new InvalidOperationException($"{nameof(lyric)} cannot be empty.");

        const int min_index = 0;
        int maxIndex = lyric.Length - 1;

        int newStartIndex = Math.Clamp(rubyTag.StartIndex + offset, min_index, maxIndex);
        int newEndIndex = Math.Clamp(rubyTag.EndIndex + offset, min_index, maxIndex);
        return new Tuple<int, int>(Math.Min(newStartIndex, newEndIndex), Math.Max(newStartIndex, newEndIndex));
    }

    public static bool OutOfRange(RubyTag rubyTag, string lyric)
    {
        return OutOfRange(lyric, rubyTag.StartIndex) || OutOfRange(lyric, rubyTag.EndIndex);
    }

    public static bool ValidNewStartIndex(RubyTag rubyTag, int newStartIndex)
    {
        ArgumentNullException.ThrowIfNull(rubyTag);

        return newStartIndex <= rubyTag.EndIndex;
    }

    public static bool ValidNewEndIndex(RubyTag rubyTag, int newEndIndex)
    {
        ArgumentNullException.ThrowIfNull(rubyTag);

        return newEndIndex >= rubyTag.StartIndex;
    }

    public static bool OutOfRange(string lyric, int index)
    {
        if (string.IsNullOrEmpty(lyric))
            return true;

        const int min_index = 0;
        int maxIndex = lyric.Length - 1;

        return index < min_index || index > maxIndex;
    }

    public static bool EmptyText(RubyTag rubyTag)
    {
        return string.IsNullOrWhiteSpace(rubyTag.Text);
    }

    /// <summary>
    /// Display tag with position format
    /// </summary>
    /// <example>
    /// ka(-2~-1)<br/>
    /// ra(4~6)<br/>
    /// </example>
    /// <param name="rubyTag"></param>
    /// <returns></returns>
    public static string PositionFormattedString(RubyTag rubyTag)
    {
        string text = string.IsNullOrWhiteSpace(rubyTag.Text) ? "empty" : rubyTag.Text;
        return $"{text}({rubyTag.StartIndex} ~ {rubyTag.EndIndex})";
    }

    public static string GetTextFromLyric(RubyTag rubyTag, string lyric)
    {
        (int startIndex, int endIndex) = GetFixedIndex(rubyTag, lyric);
        return lyric.Substring(startIndex, endIndex - startIndex + 1);
    }

    public static PositionText ToPositionText(RubyTag rubyTag)
        => new(rubyTag.Text, rubyTag.StartIndex, rubyTag.EndIndex);
}
