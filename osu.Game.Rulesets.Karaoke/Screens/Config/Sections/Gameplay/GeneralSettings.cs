// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Screens.Config.Previews;
using osu.Game.Rulesets.Karaoke.Screens.Config.Previews.Gameplay;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Gameplay
{
    public class GeneralSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "General";

        public override SettingsSubsectionPreview CreatePreview() => new ShowCursorPreview();

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Show cursor while playing",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.ShowCursor)
                },
            };
        }
    }
}
