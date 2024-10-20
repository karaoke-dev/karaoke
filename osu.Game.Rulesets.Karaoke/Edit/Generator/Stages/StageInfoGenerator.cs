// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Stages;

public abstract class StageInfoGenerator<TStageInfoConfig> : PropertyGenerator<KaraokeBeatmap, StageInfo, TStageInfoConfig>
    where TStageInfoConfig : StageInfoGeneratorConfig, new()
{
    protected StageInfoGenerator(TStageInfoConfig config)
        : base(config)
    {
    }
}
