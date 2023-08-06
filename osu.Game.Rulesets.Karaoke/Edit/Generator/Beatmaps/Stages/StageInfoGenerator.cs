// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages;

public abstract class StageInfoGenerator<TStageInfoConfig> : BeatmapPropertyGenerator<StageInfo, TStageInfoConfig>
    where TStageInfoConfig : StageInfoGeneratorConfig, new()
{
    protected StageInfoGenerator(TStageInfoConfig config)
        : base(config)
    {
    }
}
