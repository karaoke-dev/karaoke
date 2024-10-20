// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Preview;

public class PreviewStageInfoGenerator : StageInfoGenerator<PreviewStageInfoGeneratorConfig>
{
    public PreviewStageInfoGenerator(PreviewStageInfoGeneratorConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(KaraokeBeatmap item)
    {
        return null;
    }

    protected override StageInfo GenerateFromItem(KaraokeBeatmap item)
    {
        return new PreviewStageInfo();
    }
}
