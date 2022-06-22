// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    public class KaraokeEditorMenu : MenuItem
    {
        public KaraokeEditorMenu(IScreen screen, string text)
            : base(text, () => openKaraokeEditor(screen))
        {
        }

        private static void openKaraokeEditor(IScreen screen)
        {
            screen?.Push(new KaraokeEditor());
        }
    }
}
