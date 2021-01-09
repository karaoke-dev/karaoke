// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class EditModeMenu : MenuItem
    {
        public EditModeMenu()
            : base("Edit mode")
        {
            Items = createMenuItems();
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
            // todo : implementation
        }
    }
}
