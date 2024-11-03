// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Karaoke.Mods;

public abstract class ModStage<TStageInfo> : ModStage
    where TStageInfo : StageInfo
{
    public override bool IsStageInfoMatched(StageInfo stageInfo)
    {
       return stageInfo is TStageInfo;
    }

    public override StageInfo GenerateDefaultStageInfo(IBeatmap beatmap)
    {
        if (beatmap is not KaraokeBeatmap karaokeBeatmap)
            throw new InvalidOperationException();

        return CreateStageInfo(karaokeBeatmap) ?? throw new InvalidOperationException();
    }

    public override void ApplyToStageInfo(StageInfo stageInfo)
    {
        if (stageInfo is not TStageInfo tStageInfo)
            throw new InvalidOperationException();

        ApplyToCurrentStageInfo(tStageInfo);
    }

    protected abstract void ApplyToCurrentStageInfo(TStageInfo stageInfo);

    protected abstract TStageInfo? CreateStageInfo(KaraokeBeatmap beatmap);
}

public abstract class ModStage : Mod, IApplicableMod
{
    public sealed override ModType Type => ModType.Conversion;

    /// <summary>
    /// Change the stage type should not affect the score.
    /// </summary>
    public override double ScoreMultiplier => 1;

    public override Type[] IncompatibleMods => new[] { typeof(ModStage) }.Except(new[] { GetType() }).ToArray();

    public abstract bool IsStageInfoMatched(StageInfo stageInfo);

    public abstract StageInfo GenerateDefaultStageInfo(IBeatmap beatmap);

    public abstract void ApplyToStageInfo(StageInfo stageInfo);
}
