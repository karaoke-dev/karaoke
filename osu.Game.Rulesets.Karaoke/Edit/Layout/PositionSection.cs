// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class PositionSection : LayoutSection
    {
        private LabelledDropdown<Anchor> alignmentDropdown;
        private LabelledSliderBar<int> horizontalMarginSliderBar;
        private LabelledSliderBar<int> verticalMarginSliderBar;
        private LabelledDropdown<KaraokeTextSmartHorizon> smartHorizonDropdown;

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                alignmentDropdown = new LabelledDropdown<Anchor>
                {
                    Label = "Anchor",
                    Description = "Anchor section",
                    RelativeSizeAxes = Axes.X,
                    Items = (Anchor[])Enum.GetValues(typeof(Anchor))
                },
                horizontalMarginSliderBar = new LabelledSliderBar<int>
                {
                    Label = "Horizontal margin",
                    Description = "Horizontal margin section",
                    RelativeSizeAxes = Axes.X,
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 500,
                        Value = 30,
                        Default = 30
                    }
                },
                verticalMarginSliderBar = new LabelledSliderBar<int>
                {
                    Label = "Vertical margin",
                    Description = "Vertical margin section",
                    RelativeSizeAxes = Axes.X,
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 500,
                        Value = 30,
                        Default = 30
                    }
                },
                smartHorizonDropdown = new LabelledDropdown<KaraokeTextSmartHorizon>
                {
                    Label = "Smart horizon",
                    Description = "Smart horizon section",
                    RelativeSizeAxes = Axes.X,
                    Items = (KaraokeTextSmartHorizon[])Enum.GetValues(typeof(KaraokeTextSmartHorizon))
                }
            };
        }
    }
}
