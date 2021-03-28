// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Input;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections
{
    public class ScoringSection : SettingsSection
    {
        public override string Header => "Scoring";

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.Gamepad
        };

        public ScoringSection()
        {
            Children = new Drawable[]
            {
                new MicrophoneSettings(),
            };
        }
    }
}
