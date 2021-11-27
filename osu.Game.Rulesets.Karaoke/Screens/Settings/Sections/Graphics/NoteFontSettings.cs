// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Graphics
{
    public class NoteFontSettings : KaraokeSettingsSubsection
    {
        protected override LocalisableString Header => "Note font";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsFont
                {
                    LabelText = "Note font",
                    Current = Config.GetBindable<FontUsage>(KaraokeRulesetSetting.NoteFont)
                },
                new SettingsCheckbox
                {
                    LabelText = "Force use default note font.",
                    TooltipText = "Override any custom font in skin or beatmap.",
                    Current = Config.GetBindable<bool>(KaraokeRulesetSetting.ForceUseDefaultNoteFont)
                }
            };
        }
    }
}
