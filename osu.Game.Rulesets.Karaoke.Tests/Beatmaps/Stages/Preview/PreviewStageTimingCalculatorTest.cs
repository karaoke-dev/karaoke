// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Stages.Preview;

public class PreviewStageTimingCalculatorTest
{
    private const int lyric_1_id = 1;
    private const int lyric_2_id = 2;
    private const int lyric_3_id = 3;
    private const int lyric_4_id = 4;
    private const int lyric_5_id = 5;

    // lyrics in the stage.
    private const int number_of_lyrics = 4;

    // offset time in the fade in/out
    private const double fading_time = 10;

    // offset time in the Lyrics arrangement.
    private const double line_moving_time = 20;
    private const double line_moving_offset_time = 30;

    [TestCase(lyric_1_id, 0)]
    [TestCase(lyric_2_id, 0)]
    [TestCase(lyric_3_id, 0)]
    [TestCase(lyric_4_id, 0)]
    [TestCase(lyric_5_id, 2000 + line_moving_offset_time * 4)] // it's the time first lyric should be disappeared.
    public void TestGetStartTime(int lyricId, double expected)
    {
        var beatmap = createBeatmap();
        var lyric = beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == lyricId);

        var calculator = createCalculator(beatmap);
        double actual = calculator.CalculateStartTime(lyric);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(lyric_1_id, 2000)]
    [TestCase(lyric_2_id, 3000)]
    [TestCase(lyric_3_id, 4000)]
    [TestCase(lyric_4_id, 5000)]
    [TestCase(lyric_5_id, 6000)]
    public void TestGetEndTime(int lyricId, double expected)
    {
        var beatmap = createBeatmap();
        var lyric = beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == lyricId);

        var calculator = createCalculator(beatmap);
        double actual = calculator.CalculateEndTime(lyric);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(lyric_1_id, new string[] { })]
    [TestCase(lyric_2_id, new[] { "0:2000" })]
    [TestCase(lyric_3_id, new[] { "1:2030", "0:3000" })]
    [TestCase(lyric_4_id, new[] { "2:2060", "1:3030", "0:4000" })]
    [TestCase(lyric_5_id, new[] { "2:3060", "1:4030", "0:5000" })]
    public void TestGetTiming(int lyricId, string[] timing)
    {
        var beatmap = createBeatmap();
        var lyric = beatmap.HitObjects.OfType<Lyric>().Single(x => x.ID == lyricId);

        var calculator = createCalculator(beatmap);
        var expected = convertKeyToDictionary(timing);
        var actual = calculator.CalculateTimings(lyric);
        Assert.AreEqual(expected, actual);
    }

    private static IDictionary<int, double> convertKeyToDictionary(IEnumerable<string> values)
        => values.ToDictionary(k => int.Parse(k.Split(':').First()), v => double.Parse(v.Split(':').Last()));

    private PreviewStageTimingCalculator createCalculator(IBeatmap beatmap)
    {
        var definition = new PreviewStageDefinition
        {
            NumberOfLyrics = number_of_lyrics,
            FadingTime = fading_time,
            LineMovingTime = line_moving_time,
            LineMovingOffsetTime = line_moving_offset_time,
        };

        return new PreviewStageTimingCalculator(beatmap, definition);
    }

    private IBeatmap createBeatmap()
    {
        var lyrics = new List<Lyric>
        {
            createLyric(lyric_1_id, 1000, 2000),
            createLyric(lyric_2_id, 2100, 3000),
            createLyric(lyric_3_id, 3100, 4000),
            createLyric(lyric_4_id, 4100, 5000),
            createLyric(lyric_5_id, 5100, 6000)
        };

        lyrics.Reverse();
        return new Beatmap
        {
            HitObjects = lyrics.OfType<HitObject>().ToList(),
        };
    }

    private static Lyric createLyric(int id, double startTime, double endTime)
    {
        return new Lyric
        {
            ID = id,
            TimeTags = new List<TimeTag>
            {
                new(new TextIndex(), startTime),
                new(new TextIndex(), endTime)
            },
        };
    }
}
