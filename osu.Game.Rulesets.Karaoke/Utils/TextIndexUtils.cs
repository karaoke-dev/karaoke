// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TextIndexUtils
    {
        public static int ToLyricIndex(TextIndex index)
        {
            if (index.State == TextIndex.IndexState.Start)
                return index.Index;

            return index.Index + 1;
        }

        public static TextIndex.IndexState ReverseState(TextIndex.IndexState state)
        {
            if (state == TextIndex.IndexState.Start)
                return TextIndex.IndexState.End;

            return TextIndex.IndexState.Start;
        }

        public static TextIndex ShiftingIndex(TextIndex originIndex, int shifting)
        {
            return new TextIndex(originIndex.Index + shifting, originIndex.State);
        }
    }
}
