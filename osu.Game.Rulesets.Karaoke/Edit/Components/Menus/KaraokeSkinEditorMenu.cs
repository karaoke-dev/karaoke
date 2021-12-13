// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Rulesets.Karaoke.Screens.Skin;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    public class KaraokeSkinEditorMenu : MenuItem
    {
        public KaraokeSkinEditorMenu(IScreen screen, ISkin skin, string text)
            : base(text, () => openKaraokeSkin(screen, skin))
        {
        }

        private static void openKaraokeSkin(IScreen screen, ISkin skin)
        {
            screen?.Push(new KaraokeSkinEditor(skin));
        }
    }
}
