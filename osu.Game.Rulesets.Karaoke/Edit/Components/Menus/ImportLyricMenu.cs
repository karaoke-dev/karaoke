// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    public class ImportLyricMenu : MenuItem
    {
        public ImportLyricMenu(IScreen screen, string text, IBeatmapChangeHandler beatmapChangeHandler)
            : base(text, () => openLyricImporter(screen, beatmapChangeHandler))
        {
        }

        private static void openLyricImporter(IScreen screen, IBeatmapChangeHandler beatmapChangeHandler)
        {
            var importer = new LyricImporter();
            importer.OnImportFinished += beatmapChangeHandler.Import;
            screen?.Push(importer);
        }
    }
}
