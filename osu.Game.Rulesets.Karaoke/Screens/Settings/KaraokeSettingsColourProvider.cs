// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Sections;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings
{
    public class KaraokeSettingsColourProvider
    {
        public Color4 GetContentColour(SettingsSection section) => getColour(section, 0.4f, 0.6f);

        public Color4 GetContent2Colour(SettingsSection section) => getColour(section, 0.4f, 0.9f);

        public Color4 GetBackgroundColour(SettingsSection section) => getColour(section, 0.1f, 0.4f);

        public Color4 GetBackground2Colour(SettingsSection section) => getColour(section, 0.1f, 0.2f);

        public Color4 GetBackground3Colour(SettingsSection section) => getColour(section, 0.1f, 0.15f);

        private Color4 getColour(SettingsSection section, float saturation, float lightness) => Color4.FromHsl(new Vector4(getBaseHue(section), saturation, lightness, 1));

        private static float getBaseHue(SettingsSection section)
        {
            return section switch
            {
                ConfigSection => 200 / 360f, // Blue
                StyleSection => 333 / 360f, // Pink
                ScoringSection => 45 / 360f, // Orange
                null => 320 / 360f, // Plum
                _ => throw new ArgumentException($@"{section} colour scheme does not provide a hue value in {nameof(getBaseHue)}.")
            };
        }
    }
}
