// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

internal partial class NoteFontSection : StyleSection
{
    private readonly LabelledColourSelector textColorPicker;
    private readonly LabelledSwitchButton boldSwitchButton;

    protected override LocalisableString Title => "Font";

    public NoteFontSection()
    {
        Children = new Drawable[]
        {
            textColorPicker = new LabelledColourSelector
            {
                Label = "Color",
                Description = "Select color.",
            },
            boldSwitchButton = new LabelledSwitchButton
            {
                Label = "Bold",
                Description = "Select bold or not.",
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(SkinManager manager)
    {
    }
}
