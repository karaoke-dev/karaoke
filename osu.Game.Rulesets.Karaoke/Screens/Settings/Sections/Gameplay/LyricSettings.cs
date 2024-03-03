// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews;
using osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Sections.Gameplay;

public partial class LyricSettings : KaraokeSettingsSubsection
{
    protected override LocalisableString Header => "Lyric";

    public override SettingsSubsectionPreview CreatePreview() => new LyricPreview();

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            new SettingsEnumDropdown<LyricDisplayType>
            {
                LabelText = "Display type",
                Current = Config.GetBindable<LyricDisplayType>(KaraokeRulesetSetting.DisplayType),
            },
            new SettingsEnumDropdown<LyricDisplayProperty>
            {
                LabelText = "Display property",
                Current = Config.GetBindable<LyricDisplayProperty>(KaraokeRulesetSetting.DisplayProperty),
            },
        };
    }
}
