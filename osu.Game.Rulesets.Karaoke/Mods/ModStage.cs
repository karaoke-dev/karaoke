// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Mods;

public abstract class ModStage<TStageInfo> : Mod, IApplicableToStageInfo
    where TStageInfo : StageInfo
{
    public sealed override ModType Type => ModType.Conversion;

    /// <summary>
    /// Change the stage type should not affect the score.
    /// </summary>
    public override double ScoreMultiplier => 1;

    public override Type[] IncompatibleMods => new[] { typeof(ModStage<TStageInfo>) }.Except(new[] { GetType() }).ToArray();

    public bool CanApply(StageInfo stageInfo)
    {
        return stageInfo is TStageInfo;
    }

    public StageInfo? CreateDefaultStageInfo(KaraokeBeatmap beatmap)
    {
        return CreateStageInfo(beatmap);
    }

    public void ApplyToStageInfo(StageInfo stageInfo)
    {
        if (stageInfo is not TStageInfo tStageInfo)
            throw new ArgumentException($"The stage info is not matched with {GetType().Name}");

        ApplyToStageInfo(tStageInfo);
    }

    protected abstract TStageInfo? CreateStageInfo(KaraokeBeatmap beatmap);

    protected abstract void ApplyToStageInfo(TStageInfo stageInfo);
}
