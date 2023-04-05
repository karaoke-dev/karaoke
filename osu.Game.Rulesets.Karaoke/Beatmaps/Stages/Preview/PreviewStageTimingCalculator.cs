// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewStageTimingCalculator
{
    public PreviewStageTimingCalculator(IBeatmap beatmap)
    {
        // todo: calculate all timing points.
    }

    public double CalculateStartTime(Lyric lyric)
    {
        // todo : do something.
        return 0;
    }

    public double CalculateEndTime(Lyric lyric)
    {
        // todo : do something.
        return 0;
    }

    public IDictionary<int, double> CalculateTimings(Lyric lyric)
    {
        // todo : do something.
        return new Dictionary<int, double>();
    }
}
