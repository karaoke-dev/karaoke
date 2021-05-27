// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections
{
    public class ConfigSection : KaraokeSettingsSection
    {
        public override string Header => "Config";

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.Cog
        };

        public ConfigSection()
        {
            Children = new Drawable[]
            {
                new GeneralSettings(),
                new NoteSettings(),
                new LyricSettings(),
                new TranslateSettings(),
            };
        }
    }
}
