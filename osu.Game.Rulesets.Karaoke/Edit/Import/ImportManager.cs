// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Import
{
    public class ImportManager : Component
    {
        public static string[] NicokaraSkinFormatExtensions { get; } = { ".nkmproj" };

        public void ImportNicokaraSkinFile(FileInfo info)
        {
            if (!info.Exists)
                throw new FileNotFoundException("Nicokara file does not found!");

            var isFormatMatch = NicokaraSkinFormatExtensions.Contains(info.Extension);
            if (isFormatMatch)
                throw new FileLoadException("Nicokara skin extension should be .nkmproj");
        }
    }
}
