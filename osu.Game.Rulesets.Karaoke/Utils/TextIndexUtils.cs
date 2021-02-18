// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

        public static TextIndex.IndexState ReverseState(TextIndex.IndexState state)
        {
            return state == TextIndex.IndexState.Start ? TextIndex.IndexState.End : TextIndex.IndexState.Start;
        }

        public static TextIndex ShiftingIndex(TextIndex originIndex, int shifting)
        {
            return new TextIndex(originIndex.Index + shifting, originIndex.State);
        }

        /// <summary>
        /// Display string with position format
        /// </summary>
        /// <example>
        /// 3
        /// 4(end)
        /// </example>
        /// <param name="timeTag"></param>
        /// <returns></returns>
        public static string PositionFormattedString(TextIndex textIndex)
        {
            var index = textIndex.Index;
            var state = textIndex.State == TextIndex.IndexState.End ? "(end)" : "";
            return $"{index}{state}";
        }
    }
}
