// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style
{
    internal class LyricColorSection : StyleSection
    {
        private LabelledEnumDropdown<ColorArea> colorAreaDropdown;
        private LabelledColourSelector colorPicker;

        protected override string Title => "Color";

        [BackgroundDependencyLoader]
        private void load(StyleManager manager)
        {
            Children = new Drawable[]
            {
                colorAreaDropdown = new LabelledEnumDropdown<ColorArea>
                {
                    Label = "Color area",
                    Description = "Select the area you wish to adjust.",
                },
                colorPicker = new LabelledColourSelector
                {
                    Label = "Color",
                    Description = "Select color.",
                }
            };
        }
    }

    public enum ColorArea
    {
        Front_Text,

        Front_Border,

        Front_Shadow,

        Back_Text,

        Back_Border,

        Back_Shadow
    }
}
