// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.UI.HUD;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    /// <summary>
    /// An interface for mods that apply changes to the <see cref="ISettingHUDOverlay"/>.
    /// </summary>
    public interface IApplicableToSettingHUDOverlay : IApplicableMod
    {
        /// <summary>
        /// Provide a <see cref="ISettingHUDOverlay"/>. Called once on initialisation of a play instance.
        /// </summary>
        void ApplyToOverlay(ISettingHUDOverlay overlay);
    }
}
