// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapStagesChangeHandler : BeatmapPropertyChangeHandler, IBeatmapStagesChangeHandler
{
    public void AddStageInfoToBeatmap<TStageInfo>() where TStageInfo : StageInfo, new()
    {
        PerformBeatmapChanged(beatmap =>
        {
            var stage = getStageInfo<TStageInfo>(beatmap);
            if (stage != null)
                throw new InvalidOperationException($"{nameof(TStageInfo)} already exist in the beatmap.");

            beatmap.StageInfos.Add(new TStageInfo());
        });
    }

    public void RemoveStageInfoFromBeatmap<TStageInfo>() where TStageInfo : StageInfo, new()
    {
        PerformBeatmapChanged(beatmap =>
        {
            var stage = getStageInfo<TStageInfo>(beatmap);
            if (stage == null)
                throw new InvalidOperationException($"There's no {nameof(TStageInfo)} in the beatmap.");

            beatmap.StageInfos.Remove(stage);

            // Should clear the current stage info if stage is removed.
            // Beatmap processor will load the suitable stage info.
            if (beatmap.CurrentStageInfo == stage)
            {
                beatmap.CurrentStageInfo = null!;
            }
        });
    }

    private TStageInfo? getStageInfo<TStageInfo>(KaraokeBeatmap beatmap) where TStageInfo : StageInfo
        => beatmap.StageInfos.OfType<TStageInfo>().FirstOrDefault();
}
