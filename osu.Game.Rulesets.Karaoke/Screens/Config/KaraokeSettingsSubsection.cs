// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Config.Previews;

namespace osu.Game.Rulesets.Karaoke.Screens.Config
{
    public abstract class KaraokeSettingsSubsection : SettingsSubsection
    {
        [Resolved]
        protected KaraokeRulesetConfigManager Config { get; private set; }

        [Resolved]
        private Bindable<SettingsSubsection> selectedSubsection { get; set; }

        public virtual SettingsSubsectionPreview CreatePreview() => new UnderConstructionPreview("Oops");

        protected override bool OnHover(HoverEvent e)
        {
            selectedSubsection.Value = this;
            return base.OnHover(e);
        }
    }
}
