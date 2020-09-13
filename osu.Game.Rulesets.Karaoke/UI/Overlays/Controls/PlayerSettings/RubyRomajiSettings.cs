// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class RubyRomajiSettings : PlayerSettingsGroup
    {
        private readonly PlayerCheckbox displayRubyCheckBox;
        private readonly PlayerCheckbox displayRomajiCheckBox;

        public RubyRomajiSettings()
            : base("Ruby/Romaji")
        {
            Children = new Drawable[]
            {
                displayRubyCheckBox = new PlayerCheckbox
                {
                    LabelText = "Display ruby"
                },
                displayRomajiCheckBox = new PlayerCheckbox
                {
                    LabelText = "Display romaji"
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeSessionStatics session)
        {
            // Ruby/Romaji
            displayRubyCheckBox.Current = session.GetBindable<bool>(KaraokeRulesetSession.DisplayRuby);
            displayRomajiCheckBox.Current = session.GetBindable<bool>(KaraokeRulesetSession.DisplayRomaji);
        }
    }
}
