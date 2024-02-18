// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Screens.Play.PlayerSettings;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

public partial class LyricDisplaySettings : PlayerSettingsGroup
{
    private readonly PlayerEnumDropdown<LyricDisplayType> displayTypeDropdown;
    private readonly PlayerEnumDropdown<LyricDisplayProperty> displayPropertyDropdown;

    public LyricDisplaySettings()
        : base("Lyric display type")
    {
        Children = new Drawable[]
        {
            displayTypeDropdown = new PlayerEnumDropdown<LyricDisplayType>
            {
                LabelText = "Display type",
            },
            displayPropertyDropdown = new PlayerEnumDropdown<LyricDisplayProperty>
            {
                LabelText = "Display property",
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeSessionStatics session)
    {
        // Lyric display type
        displayTypeDropdown.Current = session.GetBindable<LyricDisplayType>(KaraokeRulesetSession.DisplayType);
        displayPropertyDropdown.Current = session.GetBindable<LyricDisplayProperty>(KaraokeRulesetSession.DisplayProperty);
    }
}
