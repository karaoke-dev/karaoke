// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public class SettingsMicrophoneDeviceDropdown : SettingsDropdown<string>
    {
        protected override OsuDropdown<string> CreateDropdown() => new MicrophoneDeviceDropdownControl();

        private class MicrophoneDeviceDropdownControl : DropdownControl
        {
            protected override LocalisableString GenerateItemText(string item)
                => string.IsNullOrEmpty(item) ? "Default" : base.GenerateItemText(item);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var microphoneManager = new MicrophoneManager();
            Items = microphoneManager.MicrophoneDeviceNames;
        }
    }
}
