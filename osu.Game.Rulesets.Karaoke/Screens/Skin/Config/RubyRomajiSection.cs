// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    internal class RubyRomajiSection : LyricConfigSection
    {
        private LabelledEnumDropdown<LyricTextAlignment> rubyAlignmentDropdown;
        private LabelledEnumDropdown<LyricTextAlignment> romajiAlignmentDropdown;
        private LabelledRealTimeSliderBar<int> rubyMarginSliderBar;
        private LabelledRealTimeSliderBar<int> romajiMarginSliderBar;

        protected override string Title => "Ruby/Romaji";

        [BackgroundDependencyLoader]
        private void load(LyricConfigManager manager)
        {
            Children = new Drawable[]
            {
                rubyAlignmentDropdown = new LabelledEnumDropdown<LyricTextAlignment>
                {
                    Label = "Ruby alignment",
                    Description = "Ruby alignment section",
                },
                romajiAlignmentDropdown = new LabelledEnumDropdown<LyricTextAlignment>
                {
                    Label = "Romaji alignment",
                    Description = "Romaji alignment section",
                },
                rubyMarginSliderBar = new LabelledRealTimeSliderBar<int>
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
                romajiMarginSliderBar = new LabelledRealTimeSliderBar<int>
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

            manager.LoadedLyricConfig.BindValueChanged(e =>
            {
                var lyricConfig = e.NewValue;
                applyCurrent(rubyAlignmentDropdown.Current, lyricConfig.RubyAlignment);
                applyCurrent(romajiAlignmentDropdown.Current, lyricConfig.RomajiAlignment);
                applyCurrent(rubyMarginSliderBar.Current, lyricConfig.RubyMargin);
                applyCurrent(romajiMarginSliderBar.Current, lyricConfig.RomajiMargin);

                static void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            rubyAlignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RubyAlignment = x.NewValue));
            romajiAlignmentDropdown.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RomajiAlignment = x.NewValue));
            rubyMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RubyMargin = x.NewValue));
            romajiMarginSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RomajiMargin = x.NewValue));
        }
    }
}
