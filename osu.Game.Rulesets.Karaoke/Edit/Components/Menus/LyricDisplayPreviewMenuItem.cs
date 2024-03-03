// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus;

public class LyricDisplayPreviewMenuItem : MenuItem
{
    private readonly Bindable<LyricDisplayType> bindableDisplayTypeToggle = new();
    private readonly Bindable<LyricDisplayProperty> bindableDisplayPropertyToggle = new();
    private readonly Bindable<bool> bindableDisplayTranslateToggle = new();

    public LyricDisplayPreviewMenuItem(KaraokeRulesetEditConfigManager config, string text)
        : base(text)
    {
        // Note: cannot use config.GetBindable<bool> directly the menu item.
        config.BindWith(KaraokeRulesetEditSetting.DisplayType, bindableDisplayTypeToggle);
        config.BindWith(KaraokeRulesetEditSetting.DisplayProperty, bindableDisplayPropertyToggle);
        config.BindWith(KaraokeRulesetEditSetting.DisplayTranslate, bindableDisplayTranslateToggle);

        Items = new MenuItem[]
        {
            new BindableEnumMenuItem<LyricDisplayType>("Display type", bindableDisplayTypeToggle),
            new BindableEnumMenuItem<LyricDisplayProperty>("Display property", bindableDisplayPropertyToggle),
            new BindableBoolMenuItem("Display translate", bindableDisplayTranslateToggle),
        };
    }
}
