// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Utils
{
    public static class TimeTagIndexUtils
    {
        public static int ToLyricIndex(TimeTagIndex index)
        {
            if (index.State == TimeTagIndex.IndexState.Start)
                return index.Index;

            return index.Index + 1;
        }

        public static TimeTagIndex.IndexState ReverseState(TimeTagIndex.IndexState state)
        {
            if (state == TimeTagIndex.IndexState.Start)
                return TimeTagIndex.IndexState.End;

            return TimeTagIndex.IndexState.Start;
        }
    }
}
