// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class EditModeMenu : MenuItem
    {
        private readonly Bindable<EditMode> bindableEditMode = new Bindable<EditMode>();

        public EditModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(text)
        {
            Items = createMenuItems();

            config.BindWith(KaraokeRulesetEditSetting.EditMode, bindableEditMode);
            bindableEditMode.BindValueChanged(e =>
            {
                var newSelection = e.NewValue;
                Items.OfType<ToggleMenuItem>().ForEach(x =>
                {
                    var match = x.Text.Value == getName(newSelection);
                    x.State.Value = match;
                });
            }, true);
        }

        private ToggleMenuItem[] createMenuItems()
        {
            var enums = (EditMode[])Enum.GetValues(typeof(EditMode));
            return enums.Select(e =>
            {
                var item = new ToggleMenuItem(getName(e), MenuItemType.Standard, _ => updateMode(e));
                return item;
            }).ToArray();
        }

        private string getName(EditMode mode)
        {
            switch (mode)
            {
                case EditMode.LyricEditor:
                    return "Lyric editor";

                case EditMode.Note:
                    return "Note editor";

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        private void updateMode(EditMode mode)
        {
            bindableEditMode.Value = mode;
        }
    }
}
