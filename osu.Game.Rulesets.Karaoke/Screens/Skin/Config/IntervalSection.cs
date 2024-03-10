// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config;

internal partial class IntervalSection : LyricConfigSection
{
    private readonly LabelledRealTimeSliderBar<int> lyricIntervalSliderBar;
    private readonly LabelledRealTimeSliderBar<int> rubyIntervalSliderBar;
    private readonly LabelledRealTimeSliderBar<int> romajiIntervalSliderBar;

    protected override LocalisableString Title => "Interval";

    public IntervalSection()
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
                    Default = 10,
                },
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
                    Default = 10,
                },
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
                    Default = 10,
                },
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(LyricFontInfoManager lyricFontInfoManager)
    {
        lyricFontInfoManager.LoadedLyricFontInfo.BindValueChanged(e =>
        {
            var lyricFontInfo = e.NewValue;
            applyCurrent(lyricIntervalSliderBar.Current, lyricFontInfo.LyricsInterval);
            applyCurrent(rubyIntervalSliderBar.Current, lyricFontInfo.RubyInterval);
            applyCurrent(romajiIntervalSliderBar.Current, lyricFontInfo.RomanisationInterval);

            static void applyCurrent<T>(Bindable<T> bindable, T value)
                => bindable.Value = bindable.Default = value;
        }, true);

        lyricIntervalSliderBar.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.LyricsInterval = x.NewValue));
        rubyIntervalSliderBar.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.RubyInterval = x.NewValue));
        romajiIntervalSliderBar.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.RomanisationInterval = x.NewValue));
    }
}
