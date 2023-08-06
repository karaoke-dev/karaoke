// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Stages.Preview;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Stages;

public class StageInfoGeneratorSelector<TStageInfo> : GeneratorSelector<KaraokeBeatmap, StageInfo, StageInfoGeneratorConfig>
    where TStageInfo : StageInfo
{
    public StageInfoGeneratorSelector(KaraokeRulesetEditGeneratorConfigManager generatorConfigManager)
        : base(generatorConfigManager)
    {
        registerGenerator<ClassicStageInfoGenerator, ClassicStageInfoGeneratorConfig>(typeof(ClassicStageInfo));
        registerGenerator<PreviewStageInfoGenerator, PreviewStageInfoGeneratorConfig>(typeof(PreviewStageInfo));
    }

    private void registerGenerator<TGenerator, TConfig>(Type type)
        where TGenerator : StageInfoGenerator<TConfig>
        where TConfig : StageInfoGeneratorConfig, new()
    {
        RegisterGenerator<TGenerator, TConfig>(_ => type == typeof(TStageInfo));
    }

    protected override LocalisableString? GetInvalidMessageFromItem(KaraokeBeatmap item)
    {
        if (!TryGetGenerator(item, out var generator))
            return "Sorry, the stage does not support auto-generate.";

        return generator.GetInvalidMessage(item);
    }

    protected override StageInfo GenerateFromItem(KaraokeBeatmap item)
    {
        if (!TryGetGenerator(item, out var generator))
            throw new GeneratorNotSupportedException();

        return generator.Generate(item);
    }
}
