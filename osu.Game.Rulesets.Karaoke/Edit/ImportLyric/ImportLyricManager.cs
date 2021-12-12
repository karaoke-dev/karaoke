// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using FileInfo = System.IO.FileInfo;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public class ImportLyricManager : Component
    {
        public static string[] LyricFormatExtensions { get; } = { ".lrc", ".kar", ".txt" };

        private const string backup_lrc_name = "backup.lrc";

        [Resolved]
        private EditorBeatmap editorBeatmap { get; set; }

        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public void ImportLrcFile(FileInfo info)
        {
            if (!info.Exists)
                throw new FileNotFoundException("Lyric file does not found!");

            bool isFormatMatch = LyricFormatExtensions.Contains(info.Extension);
            if (!isFormatMatch)
                throw new FileLoadException("Only .lrc or .kar karaoke file is supported now");

            var set = beatmap.Value.BeatmapSetInfo;
            var oldFile = set.Files?.FirstOrDefault(f => f.Filename == backup_lrc_name);

            using (var stream = info.OpenRead())
            {
                // todo : make a backup if has new lyric file.
                /*
                if (oldFile != null)
                    beatmaps.ReplaceFile(set, oldFile, stream, backup_lrc_name);
                else
                    beatmaps.AddFile(set, stream, backup_lrc_name);
                */

                // Import and replace all the file.
                using (var reader = new LineBufferedReader(stream))
                {
                    var decoder = new LrcDecoder();
                    var lrcBeatmap = decoder.Decode(reader);

                    // remove all hit objects (note and lyric) from beatmap
                    editorBeatmap.Clear();

                    // then re-add the lyric.
                    var lyrics = lrcBeatmap.HitObjects.OfType<Lyric>();
                    editorBeatmap.AddRange(lyrics);
                }
            }
        }
    }
}
