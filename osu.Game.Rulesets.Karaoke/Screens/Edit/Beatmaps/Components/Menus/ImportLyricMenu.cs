// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Components.Menus
{
    public class ImportLyricMenu : MenuItem
    {
        public ImportLyricMenu(IScreen screen, string text, IImportBeatmapChangeHandler importBeatmapChangeHandler)
            : base(text, () => openLyricImporter(screen, importBeatmapChangeHandler))
        {
        }

        private static void openLyricImporter(IScreen screen, IImportBeatmapChangeHandler importBeatmapChangeHandler)
        {
            var importer = new LyricImporter();
            importer.OnImportFinished += importBeatmapChangeHandler.Import;
            screen?.Push(importer);
        }
    }
}
