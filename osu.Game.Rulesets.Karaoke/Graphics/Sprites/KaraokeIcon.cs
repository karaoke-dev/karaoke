// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites
{
    public static class KaraokeIcon
    {
        public static IconUsage Get(int icon) => new IconUsage((char)icon, "osuFont");

        // ruleset icons in circles
        public static IconUsage RulesetKaraoke => FontAwesome.Solid.PlayCircle;

        // mod icons
        public static IconUsage ModDisableNote => FontAwesome.Solid.Eraser;
        public static IconUsage ModHiddenNote => OsuIcon.ModHidden;
        public static IconUsage ModHiddenRuby => FontAwesome.Solid.Gem;
        public static IconUsage ModPractice => FontAwesome.Solid.Music;
        public static IconUsage ModAutoPlayBySinger => FontAwesome.Solid.Music;
    }
}
