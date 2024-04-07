// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.Extensions;
using osu.Game.Overlays.Notifications;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Screens.Edit;
using SharpCompress.Archives.Zip;

namespace osu.Game.Rulesets.Karaoke.Edit.Debugging;

/// <summary>
/// Save or export the beatmap for debug.
/// Beatmap will be json format and might not be the final version.
/// </summary>
public partial class DebugBeatmapManager : Component
{
    [Resolved]
    private Storage storage { get; set; } = null!;

    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    /// <summary>
    /// Export the json beatmap only.
    /// </summary>
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
            sw.WriteLine(generateJsonBeatmap(beatmap));
        }

        exportStorage.PresentFileExternally(filename);
    }

    /// <summary>
    /// Export the whole beatmap with:
    /// 1. json format beatmap.
    /// 2. other resource like audio, background.
    /// </summary>
    public void ExportToJsonBeatmap()
    {
        // note : this is for develop testing purpose.
        // will be removed eventually
        string beatmapName = string.IsNullOrEmpty(beatmap.Name) ? "[NoName]" : beatmap.Name;
        var exportStorage = storage.GetStorageForDirectory("exports");
        string filename = $"{beatmapName}.osz";

        using (var outputStream = exportStorage.GetStream(filename, FileAccess.Write, FileMode.Create))
        {
            string beatmapText = generateJsonBeatmap(beatmap);
            new KaraokeLegacyBeatmapExporter(storage, filename, beatmapText).ExportToStream(beatmap.BeatmapInfo.BeatmapSet!, outputStream, null);
        }

        exportStorage.PresentFileExternally(filename);
    }

    private static string generateJsonBeatmap(EditorBeatmap beatmap)
    {
        var encoder = new KaraokeJsonBeatmapEncoder();
        var karaokeBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(beatmap);

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

        public override void ExportToStream(BeatmapSetInfo model, Stream outputStream, ProgressNotification? notification, CancellationToken cancellationToken = new())
        {
            // base.ExportModelTo(model, outputStream);
            using var zipArchive = ZipArchive.Create();

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
