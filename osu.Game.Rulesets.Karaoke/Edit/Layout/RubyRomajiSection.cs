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
    internal class RubyRomajiSection : LayoutSection
    {
        private LabelledDropdown<LyricTextAlignment> rubyAlignmentDropdown;
        private LabelledDropdown<LyricTextAlignment> romajiAlignmentDropdown;
        private LabelledSliderBar<int> rubyMarginSliderBar;
        private LabelledSliderBar<int> romajiMarginSliderBar;

        protected override string Title => "Ruby/Romaji";

        [BackgroundDependencyLoader]
        private void load(LayoutManager manager)
        {
            Children = new Drawable[]
            {
                rubyAlignmentDropdown = new LabelledDropdown<LyricTextAlignment>
                {
                    Label = "Ruby alignment",
                    Description = "Ruby alignment section",
                    Items = (LyricTextAlignment[])Enum.GetValues(typeof(LyricTextAlignment))
                },
                romajiAlignmentDropdown = new LabelledDropdown<LyricTextAlignment>
                {
                    Label = "Romaji alignment",
                    Description = "Romaji alignment section",
                    Items = (LyricTextAlignment[])Enum.GetValues(typeof(LyricTextAlignment))
                },
                rubyMarginSliderBar = new LabelledSliderBar<int>
                {
                    Label = "Ruby margin",
                    Description = "Ruby margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                },
                romajiMarginSliderBar = new LabelledSliderBar<int>
                {
                    Label = "Romaji margin",
                    Description = "Romaji margin section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                }
            };

            manager.CurrentLayout.BindValueChanged(e =>
            {
                var layout = e.NewValue;
                rubyAlignmentDropdown.Current.Value = layout.RubyAlignment;
                romajiAlignmentDropdown.Current.Value = layout.RomajiAlignment;
                rubyMarginSliderBar.Current.Value = layout.RubyMargin;
                romajiMarginSliderBar.Current.Value = layout.RomajiMargin;
            }, true);

            rubyAlignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.RubyAlignment = x.NewValue));
            romajiAlignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.RomajiAlignment = x.NewValue));
            rubyMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.RubyMargin = x.NewValue));
            romajiMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.RomajiMargin = x.NewValue));
        }
    }
}
