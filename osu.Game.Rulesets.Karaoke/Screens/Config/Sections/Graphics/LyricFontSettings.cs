// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics
{
    public class LyricFontSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Lyric font";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsFont
                {
                    LabelText = "Default main font"
                },
                new SettingsFont
                {
                    LabelText = "Default ruby font"
                },
                new SettingsFont
                {
                    LabelText = "Default romaji font"
                },
                new SettingsCheckbox
                {
                    LabelText = "Force use default lyric font.",
                    TooltipText = "Force use default font even has customize font in skin or beatmap.",
                },
                new SettingsFont
                {
                    LabelText = "Translate font"
                },
                new SettingsCheckbox
                {
                    LabelText = "Force use default translate font.",
                    TooltipText = "Force use default font even has customize font in skin or beatmap.",
                }
            };
        }
    }
}
