// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics
{
    public class TransparentSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Transparent";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SettingsSlider<double>
                {
                    LabelText = "Lyric playfield alpha",
                    Current = Config.GetBindable<double>(KaraokeRulesetSetting.LyricAlpha),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                },
                new SettingsSlider<double>
                {
                    LabelText = "Note playfield alpha",
                    Current = Config.GetBindable<double>(KaraokeRulesetSetting.NoteAlpha),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                },
            };
        }
    }
}
