// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Screens.Edit;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public class ImportLyricManager : Component
    {
        public static string[] LyricFormatExtensions { get; } = { ".lrc", ".kar" };

        private const string backup_lrc_name = "backup.lrc";

        [Resolved]
        private EditorBeatmap editorBeatmap { get; set; }

        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public void ImportLrcFile(FileInfo info)
        {
            if (!info.Exists)
                throw new FileNotFoundException("Lyric file does not found!");

            var isFormatMatch = LyricFormatExtensions.Contains(info.Extension);
            if (!isFormatMatch)
                throw new FileLoadException("Only .lrc or .kar karaoke file is supported now");

            var set = beatmap.Value.BeatmapSetInfo;
            var oldFile = set.Files?.FirstOrDefault(f => f.Filename == backup_lrc_name);

            using (var stream = info.OpenRead())
            {
                // todo : make a backup in case of new lyric file.
                /*
                if (oldFile != null)
                    beatmaps.ReplaceFile(set, oldFile, stream, backup_lrc_name);
                else
                    beatmaps.AddFile(set, stream, backup_lrc_name);
                */

                // Import and replace all the files.
                using (var reader = new Game.IO.LineBufferedReader(stream))
                {
                    var decoder = new LrcDecoder();
                    var lrcBeatmap = decoder.Decode(reader);

                    // todo : remove all notes and lyrics
                    // or just clear all beatmaps, not really sure if singer should be removed also?

                    // then re-add the lyric.
                }
            }
        }
    }
}
