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
    public class AutoFocusToEditLyricMenu : MenuItem
    {
        private readonly BindableInt bindableFocusRows = new BindableInt();

        public AutoFocusToEditLyricMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(text)
        {
            Items = Enumerable.Range(0, 6).Select(x => new ToggleMenuItem(getName(x), MenuItemType.Standard, _ => UpdateSelection(x))).ToList();

            config.BindWith(KaraokeRulesetEditSetting.AutoFocusToEditLyric, bindableFocusRows);
            bindableFocusRows.BindValueChanged(e =>
            {
                var newSelection = e.NewValue;
                Items.OfType<ToggleMenuItem>().ForEach(x =>
                {
                    var match = x.Text.Value == getName(newSelection);
                    x.State.Value = match;
                });
            }, true);
        }

        private string getName(int number)
        {
            if (number == 0)
                return "Disable";

            if (number == 1)
                return $"Enable";

            return $"Enable (skip {number - 1} rows)";
        }

        protected virtual void UpdateSelection(int selection)
        {
            bindableFocusRows.Value = selection;
        }
    }
}
