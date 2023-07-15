// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Localisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay;

public partial class PracticeSettings : KaraokeSettingsSubsection
{
    protected override LocalisableString Header => "Practice mode";

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            new SettingsSlider<double, TimeSlider>
            {
                LabelText = KaraokeSettingsSubsectionStrings.PracticePreemptTime,
                Current = Config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime),
            },
        };
    }
}
