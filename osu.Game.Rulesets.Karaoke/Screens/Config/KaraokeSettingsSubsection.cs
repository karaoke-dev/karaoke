// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public abstract class KaraokeSettingsSubsection : SettingsSubsection
    {
        [Resolved]
        protected KaraokeRulesetConfigManager Config { get; set; }
    }
}
