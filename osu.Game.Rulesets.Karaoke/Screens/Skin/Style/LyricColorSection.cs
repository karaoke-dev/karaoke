// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

internal partial class LyricColorSection : StyleSection
{
    private readonly LabelledEnumDropdown<ColorArea> colorAreaDropdown;
    private readonly LabelledColourSelector colorPicker;

    protected override LocalisableString Title => "Color";

    public LyricColorSection()
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

    [BackgroundDependencyLoader]
    private void load(StyleManager manager)
    {
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
