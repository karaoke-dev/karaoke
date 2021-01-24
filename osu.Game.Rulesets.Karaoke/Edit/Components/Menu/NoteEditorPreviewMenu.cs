// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class NoteEditorPreviewMenu : MenuItem
    {
        private readonly KaraokeRulesetEditConfigManager config;

        public NoteEditorPreviewMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(text)
        {
            this.config = config;

            Items = new[]
            {
                createToggleMenu("Display ruby", KaraokeRulesetEditSetting.DisplayRuby),
                createToggleMenu("Display romaji", KaraokeRulesetEditSetting.DisplayRomaji),
                createToggleMenu("Display translate", KaraokeRulesetEditSetting.DisplayTranslate),
            };
        }

        private ToggleMenuItem createToggleMenu(string menu, KaraokeRulesetEditSetting setting)
        {
            var bindable = new Bindable<bool>();
            var menuItem = new ToggleMenuItem(menu, MenuItemType.Standard, _ => bindable.Value = !bindable.Value);

            // create bindable
            bindable.BindValueChanged(e =>
            {
                menuItem.State.Value = e.NewValue;
            }, true);
            config.BindWith(setting, bindable);

            return menuItem;
        }
    }
}
