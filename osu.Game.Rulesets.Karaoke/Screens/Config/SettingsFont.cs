// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class SettingsFont : SettingsItem<FontUsage>
    {
        private OsuSpriteText labelText;

        protected override Drawable CreateControl() => new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Spacing = new Vector2(5),
            Children = new Drawable[]
            {
                labelText = new OsuSpriteText(),
                new TriangleButton
                {
                    RelativeSizeAxes = Axes.X
                }
            }
        };

        public override LocalisableString LabelText
        {
            get => labelText.Text;
            set => labelText.Text = value;
        }
    }
}
