// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.UI;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay
{
    public class NoteSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Note";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsEnumDropdown<KaraokeScrollingDirection>
                {
                    LabelText = "Scrolling direction",
                    Current = Config.GetBindable<KaraokeScrollingDirection>(KaraokeRulesetSetting.ScrollDirection)
                },
                new SettingsSlider<double, TimeSlider>
                {
                    LabelText = "Scroll speed",
                    Current = Config.GetBindable<double>(KaraokeRulesetSetting.ScrollTime)
                },
                new SettingsCheckbox
                {
                    LabelText = "Display alternative text",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.DisplayAlternativeText)
                },
            };
        }

        private class TimeSlider : OsuSliderBar<double>
        {
            public override string TooltipText => Current.Value.ToString("N0") + "ms";
        }
    }
}
