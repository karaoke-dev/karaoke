// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Karaoke.Resources.Fonts
{
    public static class KaraokeIcon
    {
        public static IconUsage Get(int icon) => new IconUsage((char)icon, "karaokeFont");

        // ruleset icons in circles
        public static IconUsage RulesetKaraoke => Get(0xe000);

        // mod icons
        public static IconUsage ModDisableNote => Get(0xe049);
        public static IconUsage ModHiddenNote => Get(0xe050);
        public static IconUsage ModHiddenRuby => Get(0xe051);
        public static IconUsage ModPractice => Get(0xe052);
    }
}
