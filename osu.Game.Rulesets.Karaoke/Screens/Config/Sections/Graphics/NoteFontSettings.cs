// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics
{
    public class NoteFontSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Note font";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsFont
                {
                    LabelText = "Note font"
                },
                new SettingsCheckbox
                {
                    LabelText = "Force use default note font.",
                    TooltipText = "Force use default font even has customize font in skin or beatmap.",
                }
            };
        }
    }
}
