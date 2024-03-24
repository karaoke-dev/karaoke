// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus;

public class LyricDisplayPreviewMenuItem : MenuItem
{
    private readonly Bindable<bool> bindableDisplayTranslateToggle = new();

    public LyricDisplayPreviewMenuItem(KaraokeRulesetEditConfigManager config, string text)
        : base(text)
    {
        // Note: cannot use config.GetBindable<bool> directly the menu item.
        config.BindWith(KaraokeRulesetEditSetting.DisplayTranslate, bindableDisplayTranslateToggle);

        Items = new MenuItem[]
        {
            new BindableBoolMenuItem("Display translate", bindableDisplayTranslateToggle),
        };
    }
}
