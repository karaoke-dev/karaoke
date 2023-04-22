// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Stages.Classic;

public class ClassicLyricTimingInfoTest
{
    #region Edit

    [Test]
    public void TestAddTimingPoint()
    {
        var timingInfo = new ClassicLyricTimingInfo();

        // Test add timing point.
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });

        Assert.AreEqual(1, timingInfo.Timings.Count);
        Assert.AreEqual(1, timingInfo.Timings[0].ID);
        Assert.AreEqual(1000, timingInfo.Timings[0].Time);
    }

    [Test]
    public void TestRemoveTimingPoint()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var timingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });

        // Test remove timing point.
        timingInfo.RemoveTimingPoint(timingPoint);

        Assert.IsEmpty(timingInfo.Timings);
    }

    [Test]
    public void TestAddToMapping()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var mappingTimingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 2000;
        });

        var lyric = new Lyric();

        // Test add to mapping.
        timingInfo.AddToMapping(mappingTimingPoint, lyric);

        var mappings = timingInfo.GetLyricTimingPoints(lyric).ToArray();
        Assert.AreEqual(1, mappings.Length);
        Assert.AreEqual(mappingTimingPoint, mappings[0]);
    }

    [Test]
    public void TestRemoveFromMapping()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var mappingTimingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 2000;
        });

        var lyric = new Lyric();

        timingInfo.AddToMapping(mappingTimingPoint, lyric);

        // Test remove mapping.
        timingInfo.RemoveFromMapping(mappingTimingPoint, lyric);

        var mappingTimingPoints = timingInfo.GetLyricTimingPoints(lyric);
        var mappingsIds = timingInfo.GetMatchedLyricIds(mappingTimingPoint);
        Assert.IsEmpty(mappingTimingPoints);
        Assert.IsEmpty(mappingsIds);
    }

    [Test]
    public void TestClearTimingPointFromMapping()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var mappingTimingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 2000;
        });

        var lyric = new Lyric();

        timingInfo.AddToMapping(mappingTimingPoint, lyric);

        // Should remove all mappings.
        timingInfo.ClearTimingPointFromMapping(mappingTimingPoint);

        var mappingTimingPoints = timingInfo.GetLyricTimingPoints(lyric);
        var mappingsIds = timingInfo.GetMatchedLyricIds(mappingTimingPoint);
        Assert.IsEmpty(mappingTimingPoints);
        Assert.IsEmpty(mappingsIds);
    }

    [Test]
    public void TestClearLyricFromMapping()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var mappingTimingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 2000;
        });

        var lyric = new Lyric();

        timingInfo.AddToMapping(mappingTimingPoint, lyric);

        // Should remove all mappings.
        timingInfo.ClearLyricFromMapping(lyric);

        var mappingTimingPoints = timingInfo.GetLyricTimingPoints(lyric);
        var mappingsIds = timingInfo.GetMatchedLyricIds(mappingTimingPoint);
        Assert.IsEmpty(mappingTimingPoints);
        Assert.IsEmpty(mappingsIds);
    }

    #endregion

    #region Query

    [Test]
    public void TestGetTimingPointOrder()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        timingInfo.Timings.AddRange(new[] { new ClassicLyricTimingPoint { Time = 1000 } });

        var existTimingPoint = timingInfo.Timings.First();
        int? existTimingPointOrder = timingInfo.GetTimingPointOrder(existTimingPoint);
        Assert.AreEqual(1, existTimingPointOrder);

        var notExistTimingPoint = new ClassicLyricTimingPoint { Time = 1000 };
        int? notExistTimingPointOrder = timingInfo.GetTimingPointOrder(notExistTimingPoint);
        Assert.IsNull(notExistTimingPointOrder);
    }

    [Test]
    public void TestGetLyricTimingPoints()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var mappingTimingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });

        var lyric = new Lyric();

        timingInfo.AddToMapping(mappingTimingPoint, lyric);

        // Get the mapping points.
        var mappingTimingPoints = timingInfo.GetLyricTimingPoints(lyric).ToArray();
        Assert.AreEqual(1, mappingTimingPoints.Length);
        Assert.AreEqual(mappingTimingPoint.ID, mappingTimingPoints[0].ID);
    }

    [TestCase("[2000,3000]:カラオケ", new double[] { 1000, 4000 }, 1000, 4000)]
    [TestCase("[2000,3000]:カラオケ", new double[] { 2000, 3000 }, 2000, 3000)]
    [TestCase("[2000,3000]:カラオケ", new double[] { 2001, 2999 }, 2001, 2999)]
    public void TestGetStartAndEndTime(string lyricText, double[] times, double? expectedStartTime, double? expectedEndTime)
    {
        // todo: should test with non start time and end time.
        var lyric = TestCaseTagHelper.ParseLyric(lyricText);
        var timingInfo = new ClassicLyricTimingInfo();

        foreach (double time in times)
        {
            var timingPoint = timingInfo.AddTimingPoint(x =>
            {
                x.Time = time;
            });
            timingInfo.AddToMapping(timingPoint, lyric);
        }

        // Test get timing info.
        var result = timingInfo.GetStartAndEndTime(lyric);
        Assert.AreEqual(expectedStartTime, result.Item1);
        Assert.AreEqual(expectedEndTime, result.Item2);
    }

    [Test]
    public void TestGetMatchedLyricIds()
    {
        var timingInfo = new ClassicLyricTimingInfo();
        var mappingTimingPoint = timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });

        var lyric = new Lyric();

        timingInfo.AddToMapping(mappingTimingPoint, lyric);

        // Get the mapping points.
        var mappingIds = timingInfo.GetMatchedLyricIds(mappingTimingPoint);
        Assert.AreEqual(new[] { lyric.ID }, mappingIds);
    }

    #endregion
}
