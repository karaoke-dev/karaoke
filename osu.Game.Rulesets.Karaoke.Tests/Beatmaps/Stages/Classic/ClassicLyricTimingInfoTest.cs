// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Stages.Classic;

public class ClassicLyricTimingInfoTest
{
    [TestCase("[2000,3000]:カラオケ", new double[] { 1000, 4000 }, 1000, 4000)]
    [TestCase("[2000,3000]:カラオケ", new double[] { 2000, 3000 }, 2000, 3000)]
    [TestCase("[2000,3000]:カラオケ", new double[] { 2001, 2999 }, 2001, 2999)]
    public void TestGetStartAndEndTime(string lyricText, double[] times, double? expectedStartTime, double? expectedEndTime)
    {
        // todo: should test with non start time and end time.
        var lyric = TestCaseTagHelper.ParseLyric(lyricText);
        var timingPoints = times.Select(x => new ClassicLyricTimingPoint
        {
            Time = x,
            LyricIds = new List<int>
            {
                lyric.ID,
            }
        });

        var timingInfo = new ClassicLyricTimingInfo();
        timingInfo.Timings.AddRange(timingPoints);

        var result = timingInfo.GetStartAndEndTime(lyric);
        Assert.AreEqual(expectedStartTime, result.Item1);
        Assert.AreEqual(expectedEndTime, result.Item2);
    }
}
