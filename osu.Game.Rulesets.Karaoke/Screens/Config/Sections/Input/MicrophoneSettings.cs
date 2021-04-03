// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Input
{
    public class MicrophoneSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Microphone";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsMicrophoneDeviceDropdown
                {
                    LabelText = "Microphone devices",
                    Current = Config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice)
                },
            };
        }
    }
}
