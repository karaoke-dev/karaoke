// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class IntervalSection : LayoutSection
    {
        private LabelledRealTimeSliderBar<int> lyricIntervalSliderBar;
        private LabelledRealTimeSliderBar<int> rubyIntervalSliderBar;
        private LabelledRealTimeSliderBar<int> romajiIntervalSliderBar;

        protected override string Title => "Interval";

        [BackgroundDependencyLoader]
        private void load(LayoutManager manager)
        {
            Children = new[]
            {
                lyricIntervalSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Lyrics interval",
                    Description = "Lyrics interval section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                },
                rubyIntervalSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Ruby interval",
                    Description = "Ruby interval section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                },
                romajiIntervalSliderBar = new LabelledRealTimeSliderBar<int>
                {
                    Label = "Romaji interval",
                    Description = "Romaji interval section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                }
            };

            manager.LoadedLayout.BindValueChanged(e =>
            {
                var layout = e.NewValue;
                applyCurrent(lyricIntervalSliderBar.Current, layout.LyricsInterval);
                applyCurrent(rubyIntervalSliderBar.Current, layout.RubyInterval);
                applyCurrent(romajiIntervalSliderBar.Current, layout.RomajiInterval);

                void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            lyricIntervalSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.LyricsInterval = x.NewValue));
            rubyIntervalSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.RubyInterval = x.NewValue));
            romajiIntervalSliderBar.Current.BindValueChanged(x => manager.ApplyCurrenyLayoutChange(l => l.RomajiInterval = x.NewValue));
        }
    }
}
