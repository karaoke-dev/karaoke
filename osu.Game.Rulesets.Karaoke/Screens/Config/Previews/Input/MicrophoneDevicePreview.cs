// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Input
{
    public class MicrophoneDevicePreview : SettingsSubsectionPreview
    {
        public MicrophoneDevicePreview()
        {

        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var microphoneBindable = Config.GetBindable<string>(KaraokeRulesetSetting.MicrophoneDevice);
            microphoneBindable.BindValueChanged(x =>
            {

            });
        }
    }
}
