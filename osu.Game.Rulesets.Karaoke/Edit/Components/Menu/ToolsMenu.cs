// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Layout;
using osu.Game.Rulesets.Karaoke.Edit.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Style;
using osu.Game.Rulesets.Karaoke.Edit.Translate;
using osu.Game.Screens.Edit.Components.Menus;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menu
{
    public class ToolsMenu : MenuItem
    {
        private IScreen screen;

        public ToolsMenu(IScreen screen, string text)
            : base(text)
        {
            this.screen = screen;
            Items = new MenuItem[]
            {
                createMenuItem<SingerScreen>("Singer manager"),
                createMenuItem<TranslateScreen>("Translate manager"),
                createMenuItem<LayoutScreen>("Layout manager"),
                createMenuItem<StyleScreen>("Style manager"),
            };
        }

        private EditorMenuItem createMenuItem<T>(string name) where T : EditorSubScreen, new()
        {
            return new EditorMenuItem("Singer manager", MenuItemType.Standard, () =>
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
