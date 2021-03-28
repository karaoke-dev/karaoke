// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections
{
    public class StyleSection : SettingsSection
    {
        public override string Header => "Style";

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = FontAwesome.Solid.PaintBrush
        };
    }
}
