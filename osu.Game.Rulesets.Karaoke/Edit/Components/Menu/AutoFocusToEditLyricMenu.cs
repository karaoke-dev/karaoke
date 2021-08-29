// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
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
        private const int disable_selection_index = -1;

        private readonly BindableBool bindableAutoFocusToEditLyric = new();
        private readonly BindableInt bindableAutoFocusToEditLyricSkipRows = new();

        public AutoFocusToEditLyricMenu(KaraokeRulesetLyricEditorConfigManager config, string text)
            : base(text)
        {
            var selections = new List<MenuItem>
            {
                new ToggleMenuItem(getName(disable_selection_index), MenuItemType.Standard, _ => updateAutoFocusToEditLyric())
            };
            selections.AddRange(Enumerable.Range(0, 4).Select(x => new ToggleMenuItem(getName(x), MenuItemType.Standard, _ => updateAutoFocusToEditLyricSkipRows(x))));
            Items = selections;

            config.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyric, bindableAutoFocusToEditLyric);
            config.BindWith(KaraokeRulesetLyricEditorSetting.AutoFocusToEditLyricSkipRows, bindableAutoFocusToEditLyricSkipRows);

            // mark disable as selected option.
            bindableAutoFocusToEditLyric.BindValueChanged(e =>
            {
                updateSelectionState();
            }, true);

            // mark line as selected option.
            bindableAutoFocusToEditLyricSkipRows.BindValueChanged(e =>
            {
                updateSelectionState();
            }, true);
        }

        private string getName(int number)
        {
            return number switch
            {
                disable_selection_index => "Disable",
                0 => "Enable",
                _ => $"Enable (skip {number} rows)"
            };
        }

        private void updateAutoFocusToEditLyric()
        {
            bindableAutoFocusToEditLyric.Value = !bindableAutoFocusToEditLyric.Value;
        }

        private void updateAutoFocusToEditLyricSkipRows(int rows)
        {
            bindableAutoFocusToEditLyric.Value = true;
            bindableAutoFocusToEditLyricSkipRows.Value = rows;
        }

        private void updateSelectionState()
        {
            var selection = bindableAutoFocusToEditLyric.Value ? bindableAutoFocusToEditLyricSkipRows.Value : disable_selection_index;
            Items.OfType<ToggleMenuItem>().ForEach(x =>
            {
                var match = x.Text.Value == getName(selection);
                x.State.Value = match;
            });
        }
    }
}
