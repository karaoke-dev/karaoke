// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class EditModeMenu : MenuItem
    {
        public EditModeMenu()
            : base("Edit mode")
        {
            Items = new[]
            {
                createMenuItem("Lyric mode", EditMode.LyricEditor),
                createMenuItem("Note", EditMode.Note),
            };
        }

        private ToggleMenuItem createMenuItem(string menuName, EditMode mode)
        {
            var item = new ToggleMenuItem($"{menuName}", MenuItemType.Standard, _ => updateMode(mode));
            return item;
        }

        private void updateMode(EditMode mode)
        {

        }
    }
}
