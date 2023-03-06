// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Checks;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;

public class PageGeneratorConfig : GeneratorConfig
{
    public double MinTime { get; set; }

    public double MaxTime { get; set; }

    public bool ClearExistPages { get; set; }

    public PageGeneratorConfig CreateDefaultConfig() => new()
    {
        MinTime = CheckBeatmapPageInfo.MIN_INTERVAL,
        MaxTime = CheckBeatmapPageInfo.MAX_INTERVAL,
        ClearExistPages = false
    };
}
