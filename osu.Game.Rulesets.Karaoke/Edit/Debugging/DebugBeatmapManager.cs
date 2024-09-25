// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.Extensions;
using osu.Game.Overlays.Notifications;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Utils;
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

    [Resolved]
    private BeatmapManager beatmapManager { get; set; } = null!;

    /// <summary>
    /// Force save the beatmap with json format.
    /// Modified from <see cref="BeatmapManager.Save"/>
    /// </summary>
    public void OverrideTheBeatmapWithJsonFormat()
    {
        var karaokeBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(beatmap);
        save(beatmap.BeatmapInfo, karaokeBeatmap);
    }

    /// <summary>
    /// Save the beatmap with json format to new difficulty.
    /// Modified from <see cref="BeatmapManager.CopyExistingDifficulty"/>
    /// </summary>
    public void SaveToNewDifficulty()
    {
        var referenceWorkingBeatmap = beatmap;
        var targetBeatmapSet = beatmap.BeatmapInfo.BeatmapSet;

        if (targetBeatmapSet == null)
        {
            return;
        }

        // start modifiey
        var newBeatmap = EditorBeatmapUtils.GetPlayableBeatmap(beatmap);
        BeatmapInfo newBeatmapInfo;

        newBeatmap.BeatmapInfo = newBeatmapInfo = referenceWorkingBeatmap.BeatmapInfo.Clone();
        // assign a new ID to the clone.
        newBeatmapInfo.ID = Guid.NewGuid();
        // add "(copy)" suffix to difficulty name, and additionally ensure that it doesn't conflict with any other potentially pre-existing copies.
        newBeatmapInfo.DifficultyName = NamingUtils.GetNextBestName(
            targetBeatmapSet.Beatmaps.Select(b => b.DifficultyName),
            $"{newBeatmapInfo.DifficultyName} (copy)");
        // clear the hash, as that's what is used to match .osu files with their corresponding realm beatmaps.
        newBeatmapInfo.Hash = string.Empty;
        // clear online properties.
        newBeatmapInfo.ResetOnlineInfo();

        addDifficultyToSet(targetBeatmapSet, newBeatmap);
        return;

        void addDifficultyToSet(BeatmapSetInfo targetBeatmapSet, IBeatmap newBeatmap)
        {
            // populate circular beatmap set info <-> beatmap info references manually.
            // several places like `Save()` or `GetWorkingBeatmap()`
            // rely on them being freely traversable in both directions for correct operation.
            targetBeatmapSet.Beatmaps.Add(newBeatmap.BeatmapInfo);
            newBeatmap.BeatmapInfo.BeatmapSet = targetBeatmapSet;

            save(newBeatmap.BeatmapInfo, newBeatmap);
        }
    }

    /// <summary>
    /// Copied from <see cref="BeatmapManager.Save"/>
    /// </summary>
    /// <param name="beatmapInfo"></param>
    /// <param name="beatmapContent"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void save(BeatmapInfo beatmapInfo, IBeatmap beatmapContent)
    {
        // get realm from beatmapManager using reflection
        if (beatmapManager.GetType().GetProperty("Realm", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(beatmapManager) is not RealmAccess realm)
        {
            throw new InvalidOperationException();
        }

        var setInfo = beatmapInfo.BeatmapSet;
        Debug.Assert(setInfo != null);

        // Difficulty settings must be copied first due to the clone in `Beatmap<>.BeatmapInfo_Set`.
        // This should hopefully be temporary, assuming said clone is eventually removed.

        // Warning: The directionality here is important. Changes have to be copied *from* beatmapContent (which comes from editor and is being saved)
        // *to* the beatmapInfo (which is a database model and needs to receive values without the taiko slider velocity multiplier for correct operation).
        // CopyTo() will undo such adjustments, while CopyFrom() will not.
        beatmapContent.Difficulty.CopyTo(beatmapInfo.Difficulty);

        // All changes to metadata are made in the provided beatmapInfo, so this should be copied to the `IBeatmap` before encoding.
        beatmapContent.BeatmapInfo = beatmapInfo;

        // Since now this is a locally-modified beatmap, we also set all relevant flags to indicate this.
        // Importantly, the `ResetOnlineInfo()` call must happen before encoding, as online ID is encoded into the `.osu` file,
        // which influences the beatmap checksums.
        beatmapInfo.LastLocalUpdate = DateTimeOffset.Now;
        beatmapInfo.Status = BeatmapOnlineStatus.LocallyModified;
        beatmapInfo.ResetOnlineInfo();

        realm.Write(r =>
        {
            using var stream = new MemoryStream();

            using (var sw = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                sw.WriteLine(generateJsonBeatmap(beatmapContent));
            }

            stream.Seek(0, SeekOrigin.Begin);

            // AddFile generally handles updating/replacing files, but this is a case where the filename may have also changed so let's delete for simplicity.
            var existingFileInfo = beatmapInfo.Path != null ? setInfo.GetFile(beatmapInfo.Path) : null;
            string targetFilename = createBeatmapFilenameFromMetadata(beatmapInfo);

            // ensure that two difficulties from the set don't point at the same beatmap file.
            if (setInfo.Beatmaps.Any(b => b.ID != beatmapInfo.ID && string.Equals(b.Path, targetFilename, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"{setInfo.GetDisplayString()} already has a difficulty with the name of '{beatmapInfo.DifficultyName}'.");

            if (existingFileInfo != null)
                beatmapManager.DeleteFile(setInfo, existingFileInfo);

            string oldMd5Hash = beatmapInfo.MD5Hash;

            beatmapInfo.MD5Hash = stream.ComputeMD5Hash();
            beatmapInfo.Hash = stream.ComputeSHA2Hash();

            beatmapManager.AddFile(setInfo, stream, createBeatmapFilenameFromMetadata(beatmapInfo));

            // beatmapManager.updateHashAndMarkDirty(setInfo);
            var method = typeof(BeatmapManager).GetMethod("updateHashAndMarkDirty", BindingFlags.Instance | BindingFlags.NonPublic);
            method?.Invoke(beatmapManager, new object?[] { setInfo });

            var liveBeatmapSet = r.Find<BeatmapSetInfo>(setInfo.ID)!;

            setInfo.CopyChangesToRealm(liveBeatmapSet);

            liveBeatmapSet.Beatmaps.Single(b => b.ID == beatmapInfo.ID)
                          .UpdateLocalScores(r);
        });

        Debug.Assert(beatmapInfo.BeatmapSet != null);

        static string createBeatmapFilenameFromMetadata(BeatmapInfo beatmapInfo)
        {
            var metadata = beatmapInfo.Metadata;
            return $"{metadata.Artist} - {metadata.Title} ({metadata.Author.Username}) [{beatmapInfo.DifficultyName}].osu".GetValidFilename();
        }
    }

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

    private static string generateJsonBeatmap(IBeatmap beatmap)
    {
        var encoder = new KaraokeJsonBeatmapEncoder();

        var encodeBeatmap = new Beatmap
        {
            Difficulty = beatmap.Difficulty.Clone(),
            BeatmapInfo = beatmap.BeatmapInfo.Clone(),
            ControlPointInfo = beatmap.ControlPointInfo.DeepClone(),
            Breaks = beatmap.Breaks,
            HitObjects = beatmap.HitObjects.ToList(),
        };
        encodeBeatmap.BeatmapInfo.BeatmapSet = new BeatmapSetInfo();
        encodeBeatmap.BeatmapInfo.Metadata = new BeatmapMetadata
        {
            Title = "json beatmap",
            AudioFile = beatmap.Metadata.AudioFile,
            BackgroundFile = beatmap.Metadata.BackgroundFile,
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
