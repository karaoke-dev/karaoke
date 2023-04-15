// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;

public class PreviewStageTimingCalculator
{
    private readonly Lyric[] orderedLyrics;

    // lyrics in the stage.
    private readonly int numberOfLyrics;

    // offset time in the fade in/out
    private readonly double fadingTime;

    // offset time in the Lyrics arrangement.
    private readonly double lineMovingOffsetTime;

    public PreviewStageTimingCalculator(IBeatmap beatmap, PreviewStageDefinition definition)
    {
        orderedLyrics = beatmap.HitObjects.OfType<Lyric>().OrderBy(x => x.LyricStartTime).ToArray();
        numberOfLyrics = definition.NumberOfLyrics;
        fadingTime = definition.FadingTime;
        lineMovingOffsetTime = definition.LineMovingOffsetTime;
    }

    public double CalculateStartTime(Lyric lyric)
    {
        var matchedLyrics = getRelatedLyrics(lyric, numberOfLyrics + 1).ToArray();

        // if true, means those lyrics show at the screening at the beginning.
        bool showAtBeginning = matchedLyrics.Length <= numberOfLyrics;

        if (showAtBeginning)
        {
            return 0;
        }

        double startEffectTime = matchedLyrics.Min(x => x.LyricEndTime) + numberOfLyrics * lineMovingOffsetTime;
        return startEffectTime + fadingTime;
    }

    public double CalculateEndTime(Lyric lyric)
    {
        return lyric.LyricEndTime;
    }

    /// <summary>
    /// Calculate the line and the timing the lyric should move-up.
    /// </summary>
    /// <param name="lyric"></param>
    /// <returns>The value should start from 0</returns>
    public IDictionary<int, double> CalculateTimings(Lyric lyric)
    {
        var matchedLyrics = getRelatedLyrics(lyric, numberOfLyrics).ToArray();
        var dictionary = new Dictionary<int, double>();

        // Should not include the current lyric.
        for (int i = 0; i < matchedLyrics.Length - 1; i++)
        {
            // line should start from zero.
            int line = matchedLyrics.Length - i - 2;
            double time = matchedLyrics[i].LyricEndTime + line * lineMovingOffsetTime;

            dictionary.Add(line, time);
        }

        return dictionary;
    }

    /// <summary>
    /// will take the current lyric and the previous n lyrics.
    /// note that the order should be p3, p2, p1, p0, current.
    /// </summary>
    /// <param name="lyric"></param>
    /// <param name="take">if the number is 5, means will get p3, p2, p1, p0 and current</param>
    /// <returns></returns>
    private IEnumerable<Lyric> getRelatedLyrics(Lyric lyric, int take)
        => orderedLyrics.Reverse().SkipWhile(x => x != lyric).Take(take).Reverse().ToArray();
}
