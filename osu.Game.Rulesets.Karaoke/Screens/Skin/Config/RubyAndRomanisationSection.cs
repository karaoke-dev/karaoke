﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Config;

internal partial class RubyAndRomanisationSection : LyricConfigSection
{
    private readonly LabelledEnumDropdown<LyricTextAlignment> rubyAlignmentDropdown;
    private readonly LabelledEnumDropdown<LyricTextAlignment> romanisationAlignmentDropdown;
    private readonly LabelledRealTimeSliderBar<int> rubyMarginSliderBar;
    private readonly LabelledRealTimeSliderBar<int> romanisationMarginSliderBar;

    protected override LocalisableString Title => "Ruby/Romanisation";

    public RubyAndRomanisationSection()
    {
        Children = new Drawable[]
        {
            rubyAlignmentDropdown = new LabelledEnumDropdown<LyricTextAlignment>
            {
                Label = "Ruby alignment",
                Description = "Ruby alignment section",
            },
            romanisationAlignmentDropdown = new LabelledEnumDropdown<LyricTextAlignment>
            {
                Label = "Romanisation alignment",
                Description = "Romanisation alignment section",
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
                    Default = 10,
                },
            },
            romanisationMarginSliderBar = new LabelledRealTimeSliderBar<int>
            {
                Label = "Romanisation margin",
                Description = "Romanisation margin section",
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
            applyCurrent(rubyAlignmentDropdown.Current, lyricFontInfo.RubyAlignment);
            applyCurrent(romanisationAlignmentDropdown.Current, lyricFontInfo.RomanisationAlignment);
            applyCurrent(rubyMarginSliderBar.Current, lyricFontInfo.RubyMargin);
            applyCurrent(romanisationMarginSliderBar.Current, lyricFontInfo.RomanisationMargin);

            static void applyCurrent<T>(Bindable<T> bindable, T value)
                => bindable.Value = bindable.Default = value;
        }, true);

        rubyAlignmentDropdown.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.RubyAlignment = x.NewValue));
        romanisationAlignmentDropdown.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.RomanisationAlignment = x.NewValue));
        rubyMarginSliderBar.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.RubyMargin = x.NewValue));
        romanisationMarginSliderBar.Current.BindValueChanged(x => lyricFontInfoManager.ApplyCurrentLyricFontInfoChange(l => l.RomanisationMargin = x.NewValue));
    }
}
