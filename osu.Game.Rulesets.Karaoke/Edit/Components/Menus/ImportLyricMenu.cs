// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    public class ImportLyricMenu : MenuItem
    {
        public ImportLyricMenu(IScreen screen, string text, Action<IBeatmap> importResult)
            : base(text, () => openLyricImporter(screen, importResult))
        {
        }

        private static void openLyricImporter(IScreen screen, Action<IBeatmap> importResult)
        {
            var importer = new LyricImporter();
            importer.OnImportFinished += importResult;
            screen?.Push(importer);
        }
    }
}
