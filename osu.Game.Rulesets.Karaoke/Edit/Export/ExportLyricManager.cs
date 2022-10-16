// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Screens.Edit;
using SharpCompress.Archives.Zip;

namespace osu.Game.Rulesets.Karaoke.Edit.Export
{
    public class ExportLyricManager : Component
    {
        [Resolved]
        private Storage storage { get; set; }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        public void ExportToLrc()
        {
            var exportStorage = storage.GetStorageForDirectory("lrc");
            string filename = $"{beatmap.Name}.lrc";

            using (var outputStream = exportStorage.GetStream(filename, FileAccess.Write, FileMode.Create))
            using (var sw = new StreamWriter(outputStream))
            {
                var encoder = new LrcEncoder();
                sw.WriteLine(encoder.Encode(new Beatmap
                {
                    HitObjects = beatmap.HitObjects.ToList()
                }));
            }

            exportStorage.PresentFileExternally(filename);
        }

        public void ExportToText()
        {
            var exportStorage = storage.GetStorageForDirectory("text");
            string filename = $"{beatmap.Name}.txt";

            using (var outputStream = exportStorage.GetStream(filename, FileAccess.Write, FileMode.Create))
            using (var sw = new StreamWriter(outputStream))
            {
                var encoder = new LyricTextEncoder();
                sw.WriteLine(encoder.Encode(new Beatmap
                {
                    HitObjects = beatmap.HitObjects.ToList()
                }));
            }

            exportStorage.PresentFileExternally(filename);
        }

        public void ExportToJson()
        {
            // note : this is for develop testing purpose.
            // will be removed eventually
            string beatmapName = string.IsNullOrEmpty(beatmap.Name) ? "[NoName]" : beatmap.Name;
            var exportStorage = storage.GetStorageForDirectory("json");
            string filename = $"{beatmapName}.json";

            using (var outputStream = exportStorage.GetStream(filename, FileAccess.Write, FileMode.Create))
            using (var sw = new StreamWriter(outputStream))
            {
                sw.WriteLine(generateJsonBeatmap());
            }

            exportStorage.PresentFileExternally(filename);
        }

        public void ExportToJsonBeatmap()
        {
            // note : this is for develop testing purpose.
            // will be removed eventually
            string beatmapName = string.IsNullOrEmpty(beatmap.Name) ? "[NoName]" : beatmap.Name;
            string filename = $"{beatmapName}.osu";
            string beatmapText = generateJsonBeatmap();

            new KaraokeLegacyBeatmapExporter(storage, filename, beatmapText).Export(beatmap.BeatmapInfo.BeatmapSet);
        }

        private string generateJsonBeatmap()
        {
            var encoder = new KaraokeJsonBeatmapEncoder();

            // not use editor.workingBeatmap(KaraokeBeatmap) is because karaoke beatmap is not inherit beatmap class.
            if (beatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
                throw new ArgumentNullException(nameof(karaokeBeatmap));

            var encodeBeatmap = new Beatmap
            {
                Difficulty = karaokeBeatmap.Difficulty.Clone(),
                BeatmapInfo = karaokeBeatmap.BeatmapInfo.Clone(),
                ControlPointInfo = karaokeBeatmap.ControlPointInfo.DeepClone(),
                Breaks = karaokeBeatmap.Breaks,
                HitObjects = beatmap.HitObjects.ToList(),
            };
            encodeBeatmap.BeatmapInfo.BeatmapSet = new BeatmapSetInfo();
            encodeBeatmap.BeatmapInfo.Metadata = new BeatmapMetadata
            {
                Title = "json beatmap",
                AudioFile = karaokeBeatmap.Metadata.AudioFile,
                BackgroundFile = karaokeBeatmap.Metadata.BackgroundFile,
            };

            return encoder.Encode(encodeBeatmap);
        }

        private class KaraokeLegacyBeatmapExporter : LegacyBeatmapExporter
        {
            private readonly string filename;
            private readonly string content;

            public KaraokeLegacyBeatmapExporter(Storage storage, string filename, string content)
                : base(storage)
            {
                this.filename = filename;
                this.content = content;
            }

            public override void ExportModelTo(BeatmapSetInfo model, Stream outputStream)
            {
                // base.ExportModelTo(model, outputStream);
                using (var zipArchive = ZipArchive.Create())
                {
                    foreach (INamedFileUsage file in model.Files)
                    {
                        // do not export other osu beatmap.
                        if (file.Filename.EndsWith(".osu", StringComparison.Ordinal))
                            continue;

                        zipArchive.AddEntry(file.Filename, UserFileStorage.GetStream(file.File.GetStoragePath()));
                    }

                    // add the json file.
                    using var jsonBeatmapStream = getJsonBeatmapStream();
                    zipArchive.AddEntry(filename, jsonBeatmapStream);

                    zipArchive.SaveTo(outputStream);
                }
            }

            private Stream getJsonBeatmapStream()
            {
                var memoryStream = new MemoryStream();
                var sw = new StreamWriter(memoryStream);

                sw.WriteLine(content);
                sw.Flush();

                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
