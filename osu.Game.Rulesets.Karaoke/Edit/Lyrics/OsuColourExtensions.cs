// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public static class OsuColourExtensions
    {
        public static Color4 GetTimeTagColour(this OsuColour colours, TimeTag timeTag)
        {
            var hasTime = timeTag.Time.HasValue;
            if (!hasTime)
                return colours.Gray7;

            var start = timeTag.Index.State == TextIndex.IndexState.Start;
            return start ? colours.Yellow : colours.YellowDarker;
        }
    }
}
