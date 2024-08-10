// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using FileInfo = System.IO.FileInfo;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public partial class ImportLyricManager : Component
{
    public static string[] LyricFormatExtensions { get; } = { ".lrc", ".kar", ".txt" };

    private const string backup_file_name = "backup";

    [Resolved]
    private EditorBeatmap editorBeatmap { get; set; } = null!;

    [Resolved]
    private IBindable<WorkingBeatmap> beatmap { get; set; } = null!;

    public void ImportFile(FileInfo info)
    {
        if (!info.Exists)
            throw new FileNotFoundException("Lyric file does not found!");

        bool isFormatMatch = LyricFormatExtensions.Contains(info.Extension);
        if (!isFormatMatch)
            throw new FileLoadException("Only .lrc or .kar karaoke file is supported now");

        var set = beatmap.Value.BeatmapSetInfo;
        var oldFile = set.Files.FirstOrDefault(f => f.Filename == backup_file_name);

        using var stream = info.OpenRead();

        // todo : make a backup if has new lyric file.
        /*
        if (oldFile != null)
            beatmaps.ReplaceFile(set, oldFile, stream, backup_file_name);
        else
            beatmaps.AddFile(set, stream, backup_file_name);
        */

        // Import and replace all the file.
        using var reader = new LineBufferedReader(stream);
        string content = reader.ReadToEnd();
        var lyrics = decodeLyrics(content, info.Extension);

        // remove all hit objects (note and lyric) from beatmap
        editorBeatmap.Clear();

        // then re-add the lyric.
        editorBeatmap.AddRange(lyrics);
    }

    private static Lyric[] decodeLyrics(string content, string extension)
    {
        IDecoder<Lyric[]> decoder = extension switch
        {
            ".lrc" => new LrcDecoder(),
            ".kar" => new KarDecoder(),
            ".txt" => new LyricTextDecoder(),
            _ => throw new NotSupportedException("Unsupported lyric file format"),
        };

        return decoder.Decode(content);
    }

    public void AbortImport()
    {
        editorBeatmap.Clear();
    }
}
