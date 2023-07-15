// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages.Classic;

public class ClassicStageInfoGenerator : StageInfoGenerator<ClassicStageInfoGeneratorConfig>
{
    public ClassicStageInfoGenerator(ClassicStageInfoGeneratorConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(KaraokeBeatmap item)
    {
        var lyrics = item.HitObjects.OfType<Lyric>().ToArray();
        if (!lyrics.Any())
            return "Should have lyric in the beatmap.";

        return null;
    }

    protected override StageInfo GenerateFromItem(KaraokeBeatmap item)
    {
        int lyricRowAmount = Config.LyricRowAmount.Value;

        // it's OK not to get the config in the config manager.
        var layoutCategoryGenerator = new ClassicLyricLayoutCategoryGenerator(new ClassicLyricLayoutCategoryGeneratorConfig
        {
            LyricRowAmount =
            {
                Value = lyricRowAmount,
            },
        });

        // it's OK not to get the config in the config manager.
        var timingInfoGenerator = new ClassicLyricTimingInfoGenerator(new ClassicLyricTimingInfoGeneratorConfig
        {
            LyricRowAmount =
            {
                Value = lyricRowAmount,
            },
        });

        return new ClassicStageInfo
        {
            LyricLayoutCategory = layoutCategoryGenerator.Generate(item),
            LyricTimingInfo = timingInfoGenerator.Generate(item),
        };
    }
}
