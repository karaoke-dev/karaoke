// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class PositionSection : LayoutSection
    {
        private LabelledDropdown<Anchor> alignmentDropdown;
        private LabelledRealTimeSliderBar<int> horizontalMarginSliderBar;
        private LabelledRealTimeSliderBar<int> verticalMarginSliderBar;
        private LabelledDropdown<KaraokeTextSmartHorizon> smartHorizonDropdown;

        protected override string Title => "Position";

        [BackgroundDependencyLoader]
        private void load(LayoutManager manager)
        {
            Children = new Drawable[]
            {
                alignmentDropdown = new LabelledDropdown<Anchor>
                {
                    Label = "Anchor",
                    Description = "Anchor section",
                    Items = (Anchor[])Enum.GetValues(typeof(Anchor))
                },
                horizontalMarginSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Horizontal margin",
                    Description = "Horizontal margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 500,
                        Value = 30,
                        Default = 30
                    }
                },
                verticalMarginSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Vertical margin",
                    Description = "Vertical margin section",
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
                    Items = (KaraokeTextSmartHorizon[])Enum.GetValues(typeof(KaraokeTextSmartHorizon))
                }
            };

            manager.LoadedLayout.BindValueChanged(e =>
            {
                var layout = e.NewValue;
                applyCurrent(alignmentDropdown.Current, layout.Alignment);
                applyCurrent(horizontalMarginSliderBar.Current, layout.HorizontalMargin);
                applyCurrent(verticalMarginSliderBar.Current, layout.VerticalMargin);
                applyCurrent(smartHorizonDropdown.Current, layout.SmartHorizon);

                void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            alignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.Alignment = x.NewValue));
            horizontalMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.HorizontalMargin = x.NewValue));
            verticalMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.VerticalMargin = x.NewValue));
            smartHorizonDropdown.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.SmartHorizon = x.NewValue));
        }
    }
}
