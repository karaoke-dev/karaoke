// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config
{
    internal class IntervalSection : LyricConfigSection
    {
        private LabelledRealTimeSliderBar<int> lyricIntervalSliderBar;
        private LabelledRealTimeSliderBar<int> rubyIntervalSliderBar;
        private LabelledRealTimeSliderBar<int> romajiIntervalSliderBar;

        protected override LocalisableString Title => "Interval";

        [BackgroundDependencyLoader]
        private void load(LyricConfigManager manager)
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

            manager.LoadedLyricConfig.BindValueChanged(e =>
            {
                var lyricConfig = e.NewValue;
                applyCurrent(lyricIntervalSliderBar.Current, lyricConfig.LyricsInterval);
                applyCurrent(rubyIntervalSliderBar.Current, lyricConfig.RubyInterval);
                applyCurrent(romajiIntervalSliderBar.Current, lyricConfig.RomajiInterval);

                static void applyCurrent<T>(Bindable<T> bindable, T value)
                    => bindable.Value = bindable.Default = value;
            }, true);

            lyricIntervalSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.LyricsInterval = x.NewValue));
            rubyIntervalSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RubyInterval = x.NewValue));
            romajiIntervalSliderBar.Current.BindValueChanged(x => manager.ApplyCurrentLyricConfigChange(l => l.RomajiInterval = x.NewValue));
        }
    }
}
