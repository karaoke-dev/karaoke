// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

namespace osu.Game.Rulesets.Karaoke.Mods;

public class KaraokeModClassicStage : ModStage<ClassicStageInfo>
{
    public override string Name => "Classic stage";

    public override string Acronym => "CS";

    public override LocalisableString Description => "Karaoke mod like other karaoke game or software.";

    protected override ClassicStageInfo CreateStageInfo(KaraokeBeatmap beatmap)
    {
        var config = new ClassicStageInfoGeneratorConfig();
        var generator = new ClassicStageInfoGenerator(config);

        return (ClassicStageInfo)generator.Generate(beatmap);
    }

    protected override void ApplyToStageInfo(ClassicStageInfo stageInfo)
    {
        // todo: adjust stage by config.
    }
}
