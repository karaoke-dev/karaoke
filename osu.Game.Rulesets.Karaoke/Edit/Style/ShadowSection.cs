// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class ShadowSection : StyleSection
    {
        private LabelledSwitchButton displayShaderSwitchButton;
        private LabelledRealTimeSliderBar<float> shadowXSliderBar;
        private LabelledRealTimeSliderBar<float> shadowYSliderBar;

        protected override string Title => "Shadow";

        [BackgroundDependencyLoader]
        private void load(SkinManager manager)
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
                        MaxValue = 20
                    }
                },
                shadowYSliderBar = new LabelledRealTimeSliderBar<float>
                {
                    Label = "Shadow Y",
                    Description = "Adjust shadow y position.",
                    Current = new BindableFloat
                    {
                        Value = 10,
                        MinValue = 0,
                        MaxValue = 20
                    }
                }
            };
        }
    }
}
