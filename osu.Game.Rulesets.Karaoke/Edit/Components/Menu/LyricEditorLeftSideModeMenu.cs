// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class LyricEditorLeftSideModeMenu : MenuItem
    {
        private readonly Bindable<LyricFastEditMode> bindableLyricEditorFastEditMode = new Bindable<LyricFastEditMode>();

        public LyricEditorLeftSideModeMenu(KaraokeRulesetEditConfigManager config, string text)
            : base(text)
        {
            Items = createMenuItems();

            config.BindWith(KaraokeRulesetEditSetting.LyricEditorFastEditMode, bindableLyricEditorFastEditMode);
            bindableLyricEditorFastEditMode.BindValueChanged(e =>
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
            var enums = (LyricFastEditMode[])Enum.GetValues(typeof(LyricFastEditMode));
            return enums.Select(e =>
            {
                var item = new ToggleMenuItem(getName(e), MenuItemType.Standard, _ => updateMode(e));
                return item;
            }).ToArray();
        }

        private string getName(LyricFastEditMode mode)
        {
            switch (mode)
            {
                case LyricFastEditMode.None:
                    return "None";

                case LyricFastEditMode.Layout:
                    return "Layout selection";

                case LyricFastEditMode.Singer:
                    return "Singer selection";

                case LyricFastEditMode.Language:
                    return "Language selection";

                case LyricFastEditMode.TimeTag:
                    return "Time tag display";

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        private void updateMode(LyricFastEditMode mode)
        {
            bindableLyricEditorFastEditMode.Value = mode;
        }
    }
}
