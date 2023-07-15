// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

internal partial class LyricShadowSection : StyleSection
{
    private readonly LabelledSwitchButton displayShaderSwitchButton;
    private readonly LabelledRealTimeSliderBar<float> shadowXSliderBar;
    private readonly LabelledRealTimeSliderBar<float> shadowYSliderBar;

    protected override LocalisableString Title => "Shadow";

    public LyricShadowSection()
    {
        Children = new Drawable[]
        {
            displayShaderSwitchButton = new LabelledSwitchButton
            {
                Label = "Shadow",
                Description = "Display shadow or not.",
            },
            shadowXSliderBar = new LabelledRealTimeSliderBar<float>
            {
                Label = "Shadow X",
                Description = "Adjust shadow x position.",
                Current = new BindableFloat
                {
                    Value = 10,
                    MinValue = 0,
                    MaxValue = 20,
                },
            },
            shadowYSliderBar = new LabelledRealTimeSliderBar<float>
            {
                Label = "Shadow Y",
                Description = "Adjust shadow y position.",
                Current = new BindableFloat
                {
                    Value = 10,
                    MinValue = 0,
                    MaxValue = 20,
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(SkinManager manager)
    {
    }
}
