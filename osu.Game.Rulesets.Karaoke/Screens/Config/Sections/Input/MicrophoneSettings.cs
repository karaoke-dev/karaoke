// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Config.Previews;
using osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Input;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Input
{
    public class MicrophoneSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Microphone";

        public override SettingsSubsectionPreview CreatePreview() => new MicrophoneDevicePreview();

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
