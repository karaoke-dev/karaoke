// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Style
{
    internal class LyricFontSection : StyleSection
    {
        private LabelledDropdown<Font> fontDropdown;
        private LabelledSwitchButton boldSwitchButton;
        private LabelledRealTimeSliderBar<float> fontSizeSliderBar;
        private LabelledRealTimeSliderBar<int> borderSliderBar;

        protected override string Title => "Font";

        [BackgroundDependencyLoader]
        private void load(SkinManager manager)
        {
            Children = new Drawable[]
            {
                fontDropdown = new LabelledDropdown<Font>
                {
                    Label = "Font",
                    Description = "Select display font.",
                    Items = EnumUtils.GetValues<Font>()
                },
                boldSwitchButton = new LabelledSwitchButton
                {
                    Label = "Bold",
                    Description = "Select bold or not.",
                },
                fontSizeSliderBar = new LabelledRealTimeSliderBar<float>
                {
                    Label = "Font size",
                    Description = "Adjust font size.",
                    Current = new BindableFloat
                    {
                        Value = 30,
                        MinValue = 10,
                        MaxValue = 70
                    }
                },
                borderSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Border size",
                    Description = "Adjust border size.",
                    Current = new BindableInt
                    {
                        Value = 10,
                        MinValue = 0,
                        MaxValue = 20
                    }
                }
            };
        }
    }

    public enum Font
    {
        F001,

        F002,

        F003
    }
}
