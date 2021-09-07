// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class LyricColorSection : StyleSection
    {
        private LabelledDropdown<ColorArea> colorAreaDropdown;
        private LabelledDropdown<BrushType> brushTypeDropdown;
        private LabelledColourSelector colorPicker;

        protected override string Title => "Color";

        [BackgroundDependencyLoader]
        private void load(StyleManager manager)
        {
            Children = new Drawable[]
            {
                colorAreaDropdown = new LabelledDropdown<ColorArea>
                {
                    Label = "Color area",
                    Description = "Select which area wants to be adjust",
                    Items = EnumUtils.GetValues<ColorArea>()
                },
                brushTypeDropdown = new LabelledDropdown<BrushType>
                {
                    Label = "Brush type",
                    Description = "Select brush type",
                    Items = EnumUtils.GetValues<BrushType>()
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
