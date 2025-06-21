// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModPreviewStage : ModStage<PreviewStageInfo>
{
    public override string Name => "Preview stage";

    public override string Acronym => "PS";

    public override LocalisableString Description => "Focus on preview the lyric text.";

    public override Type[] IncompatibleMods => new Type[]
    {
        typeof(KaraokeModClassicStage),
    };

    protected override PreviewStageInfo CreateStageInfo(KaraokeBeatmap beatmap)
    {
        var config = new PreviewStageInfoGeneratorConfig();
        var generator = new PreviewStageInfoGenerator(config);

        return (PreviewStageInfo)generator.Generate(beatmap);
    }

    protected override void ApplyToStageInfo(PreviewStageInfo stageInfo)
    {
        // todo: adjust stage by config.
    }
}
