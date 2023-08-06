// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Stages.Preview;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModPreviewStage : ModStage<PreviewStageInfo>
{
    public override string Name => "Preview stage";

    public override string Acronym => "PS";

    public override LocalisableString Description => "Focus on preview the lyric text.";

    protected override void ApplyToCurrentStageInfo(PreviewStageInfo stageInfo)
    {
        throw new System.NotImplementedException();
    }

    protected override PreviewStageInfo CreateStageInfo(KaraokeBeatmap beatmap)
    {
        var config = new PreviewStageInfoGeneratorConfig();
        var generator = new PreviewStageInfoGenerator(config);

        return (PreviewStageInfo)generator.Generate(beatmap);
    }
}
