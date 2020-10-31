// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class NoteFontSection : StyleSection
    {
        private ColorPicker textColorPicker;
        private LabelledSwitchButton boldSwitchButton;

        protected override string Title => "Font";

        [BackgroundDependencyLoader]
        private void load(SkinManager manager)
        {
            Children = new Drawable[]
            {
                textColorPicker = new ColorPicker
                {
                    RelativeSizeAxes = Axes.X,
                },
                boldSwitchButton = new LabelledSwitchButton
                {
                    Label = "Bold",
                    Description = "Select bold or not.",
                }
            };
        }
    }
}
