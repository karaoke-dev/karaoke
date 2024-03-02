// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus;

public class NoteEditorPreviewMenu : MenuItem
{
    private readonly Bindable<bool> bindableDisplayRubyToggle = new();
    private readonly Bindable<bool> bindableDisplayRomajiToggle = new();
    private readonly Bindable<bool> bindableDisplayTranslateToggle = new();

    public NoteEditorPreviewMenu(KaraokeRulesetEditConfigManager config, string text)
        : base(text)
    {
        // Note: cannot use config.GetBindable<bool> directly the menu item.
        config.BindWith(KaraokeRulesetEditSetting.DisplayRuby, bindableDisplayRubyToggle);
        config.BindWith(KaraokeRulesetEditSetting.DisplayRomaji, bindableDisplayRomajiToggle);
        config.BindWith(KaraokeRulesetEditSetting.DisplayTranslate, bindableDisplayTranslateToggle);

        Items = new[]
        {
            new BindableBoolMenuItem(bindableDisplayRubyToggle, "Display ruby"),
            new BindableBoolMenuItem(bindableDisplayRomajiToggle, "Display romaji"),
            new BindableBoolMenuItem(bindableDisplayTranslateToggle, "Display translate"),
        };
    }
}
