// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

public partial class ImportBeatmapChangeHandler : Component, IImportBeatmapChangeHandler
{
    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    public void Import(IBeatmap newBeatmap)
    {
        beatmap.BeginChange();

        beatmap.Clear();

        var lyrics = newBeatmap.HitObjects.OfType<Lyric>().ToArray();

        foreach (Lyric lyric in lyrics)
        {
            lyric.ID = ElementId.NewElementId();
        }

        beatmap.AddRange(lyrics);

        beatmap.EndChange();
    }
}
