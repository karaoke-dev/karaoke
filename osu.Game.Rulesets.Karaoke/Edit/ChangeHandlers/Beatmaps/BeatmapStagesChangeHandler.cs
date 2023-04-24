// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapStagesChangeHandler : BeatmapPropertyChangeHandler, IBeatmapStagesChangeHandler
{
    [Resolved, AllowNull]
    private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; }

    public void AddStageInfoToBeatmap<TStageInfo>() where TStageInfo : StageInfo, new()
    {
        PerformBeatmapChanged(beatmap =>
        {
            var stage = getStageInfo<TStageInfo>(beatmap);
            if (stage != null)
                throw new InvalidOperationException($"{nameof(TStageInfo)} already exist in the beatmap.");

            var generator = new StageInfoGeneratorSelector<TStageInfo>(generatorConfigManager);
            var stageInfo = generator.Generate(beatmap);

            beatmap.StageInfos.Add(stageInfo);
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
