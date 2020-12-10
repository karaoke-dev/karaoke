// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public class TimeTag
    {
        public TimeTag(TimeTagIndex index, double? time = null)
        {
            Item1 = index;
            Item2 = time;
        }

        public TimeTagIndex Item1 { get; }

        public double? Item2 { get; set; }
    }
}
