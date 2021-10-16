// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.HUD
{
    public interface ISettingHUDOverlay
    {
        void AddSettingsGroup(PlayerSettingsGroup group);

        void AddExtraOverlay(RightSideOverlay container);
    }
}
