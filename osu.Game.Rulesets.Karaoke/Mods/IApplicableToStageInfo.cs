// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Mods;

/// <summary>
/// An interface for mods that prefer to use the type of <see cref="StageInfo"/>.
/// Also, it can override the parameter of <see cref="StageInfo"/>.
/// </summary>
public interface IApplicableToStageInfo : IApplicableToStage
{
    StageInfo? CreateDefaultStageInfo(KaraokeBeatmap beatmap);

    void ApplyToStageInfo(StageInfo stageInfo);
}
