// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Layout;
using osu.Game.Rulesets.Karaoke.Edit.Style;
using osu.Game.Screens.Edit.Components.Menus;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class ManagerMenu : MenuItem
    {
        private readonly IScreen screen;

        public ManagerMenu(IScreen screen, string text)
            : base(text)
        {
            this.screen = screen;
            Items = new MenuItem[]
            {
                // createMenuItem<SingerScreen>("Singer"),
                // createMenuItem<TranslateScreen>("Translate"),
                createMenuItem<LayoutScreen>("Layout"),
                createMenuItem<StyleScreen>("Style"),
            };
        }

        private EditorMenuItem createMenuItem<T>(string name) where T : EditorSubScreen, new()
        {
            return new(name, MenuItemType.Standard, () =>
            {
                if (screen == null)
                    return;

                // todo : implementation
                //var newScreen = new T();
                //screen.Push(newScreen);
            });
        }
    }
}
