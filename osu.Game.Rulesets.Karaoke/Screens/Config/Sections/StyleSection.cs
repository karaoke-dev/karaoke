// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections
{
    public class StyleSection : KaraokeSettingsSection
    {
        public override LocalisableString Header => "Style";

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.PaintBrush
        };

        public StyleSection()
        {
            Children = new Drawable[]
            {
                new TransparentSettings(),
                new LyricFontSettings(),
                new NoteFontSettings(),
                new ManageFontSettings(),
            };
        }
    }
}
