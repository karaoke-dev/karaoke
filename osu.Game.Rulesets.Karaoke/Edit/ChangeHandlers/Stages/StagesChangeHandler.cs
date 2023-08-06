// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;

public partial class StagesChangeHandler : BeatmapPropertyChangeHandler, IStagesChangeHandler
{
    [Resolved]
    private KaraokeRulesetEditGeneratorConfigManager generatorConfigManager { get; set; } = null!;

    bool IAutoGenerateChangeHandler<StageInfo>.CanGenerate<TStageInfo>()
        => CanGenerate<TStageInfo>();

    public bool CanGenerate<TStageInfo>() where TStageInfo : StageInfo
    {
        return GetGeneratorNotSupportedMessage<TStageInfo>() == null;
    }

    public LocalisableString? GetGeneratorNotSupportedMessage<TStageInfo>() where TStageInfo : StageInfo
    {
        var stage = getStageInfo<TStageInfo>(KaraokeBeatmap);
        if (stage != null)
            return $"{nameof(TStageInfo)} already exist in the beatmap.";

        var generator = new StageInfoGeneratorSelector<TStageInfo>(generatorConfigManager);
        return generator.GetInvalidMessage(KaraokeBeatmap);
    }

    void IAutoGenerateChangeHandler<StageInfo>.AutoGenerate<TStageInfo>()
        => AutoGenerate<TStageInfo>();

    public void AutoGenerate<TStageInfo>() where TStageInfo : StageInfo
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

    public void Remove<TStageInfo>() where TStageInfo : StageInfo
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
