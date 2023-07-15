// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Checks;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;

public class PageGeneratorConfig : GeneratorConfig
{
    [ConfigSource("Min time", "Min interval between pages.")]
    public Bindable<double> MinTime { get; } = new BindableDouble(CheckBeatmapPageInfo.MIN_INTERVAL)
    {
        MinValue = CheckBeatmapPageInfo.MIN_INTERVAL,
        MaxValue = CheckBeatmapPageInfo.MAX_INTERVAL,
    };

    [ConfigSource("Max time", "Max interval between pages.")]
    public Bindable<double> MaxTime { get; } = new BindableDouble(CheckBeatmapPageInfo.MAX_INTERVAL)
    {
        MinValue = CheckBeatmapPageInfo.MIN_INTERVAL,
        MaxValue = CheckBeatmapPageInfo.MAX_INTERVAL,
    };

    [ConfigSource("Clear the exist page.", "Clear the exist page after generated.")]
    public Bindable<bool> ClearExistPages { get; } = new BindableBool();
}
