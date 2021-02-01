// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Screens.Edit.Components.Menus;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class GeneratorMenu : MenuItem
    {
        public GeneratorMenu(string text)
            : base(text)
        {
            Items = new MenuItem[]
            {
                createMenuItem("Language"),
                createMenuItem("Layout"),
                createMenuItem("Ruby (For japanese lyric only)"),
                createMenuItem("Romaji"),
                createMenuItem("Time tags"),
            };
        }

        private EditorMenuItem createMenuItem(string name)
        {
            return new EditorMenuItem(name, MenuItemType.Standard, () =>
            {
                // todo : implementation
            });
        }
    }
}
