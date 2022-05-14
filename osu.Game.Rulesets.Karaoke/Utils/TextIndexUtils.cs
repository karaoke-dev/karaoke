// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextIndexUtils
    {
        public static int ToStringIndex(TextIndex index)
        {
            if (index.State == TextIndex.IndexState.Start)
                return index.Index;

            return index.Index + 1;
        }

        public static TextIndex FromStringIndex(int index, bool end)
        {
            return end ? new TextIndex(index - 1, TextIndex.IndexState.End) : new TextIndex(index);
        }

        public static TextIndex.IndexState ReverseState(TextIndex.IndexState state)
        {
            return state == TextIndex.IndexState.Start ? TextIndex.IndexState.End : TextIndex.IndexState.Start;
        }

        public static TextIndex GetPreviousIndex(TextIndex originIndex)
        {
            int previousIndex = ToStringIndex(originIndex) - 1;
            var previousState = ReverseState(originIndex.State);
            return new TextIndex(previousIndex, previousState);
        }

        public static TextIndex GetNextIndex(TextIndex originIndex)
        {
            int nextIndex = ToStringIndex(originIndex);
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
            index.State switch
            {
                TextIndex.IndexState.Start => startValue,
                TextIndex.IndexState.End => endValue,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
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
            string state = textIndex.State == TextIndex.IndexState.End ? "(end)" : "";
            return $"{index}{state}";
        }
    }
}
