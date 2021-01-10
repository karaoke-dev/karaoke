// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorTextSizeMenu : MenuItem
    {
        private readonly Bindable<int> bindableFontSize = new Bindable<int>();

        public LyricEditorTextSizeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(text)
        {
            Items = createMenuItems();

            config.BindWith(KaraokeRulesetEditSetting.LyricEditorFontSize, bindableFontSize);
            bindableFontSize.BindValueChanged(e =>
            {
                var newSelection = e.NewValue;
                Items.OfType<ToggleMenuItem>().ForEach(x => {
                    var match = x.Text.Value == getName(newSelection);
                    x.State.Value = match;
                });
            }, true);
        }

        private ToggleMenuItem[] createMenuItems()
        {
            var enums = new[] { 12, 14, 16, 18, 20, 22, 24, 26, 28, 32, 36, 40, 48 };
            return enums.Select(e =>
            {
                var item = new ToggleMenuItem(getName(e), MenuItemType.Standard, _ => updateMode(e));
                return item;
            }).ToArray();
        }

        private string getName(float size)
        {
            return $"{size} px";
        }

        private void updateMode(int size)
        {
            bindableFontSize.Value = size;
        }
    }
}
