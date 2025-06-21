// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Stages.Infos.Classic;

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

        Assert.That(timingInfo.Timings.Count, Is.EqualTo(1));
        Assert.That(timingInfo.Timings[0].ID.ToString(), Is.Not.Empty);
        Assert.That(timingInfo.Timings[0].Time, Is.EqualTo(1000));
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

        Assert.That(timingInfo.Timings, Is.Empty);
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
        Assert.That(mappings.Length, Is.EqualTo(1));
        Assert.That(mappings[0], Is.EqualTo(mappingTimingPoint));
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
        Assert.That(mappingTimingPoints, Is.Empty);
        Assert.That(mappingsIds, Is.Empty);
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
        Assert.That(mappingTimingPoints, Is.Empty);
        Assert.That(mappingsIds, Is.Empty);
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
        Assert.That(mappingTimingPoints, Is.Empty);
        Assert.That(mappingsIds, Is.Empty);
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
        Assert.That(existTimingPointOrder, Is.EqualTo(1));

        var notExistTimingPoint = new ClassicLyricTimingPoint { Time = 1000 };
        int? notExistTimingPointOrder = timingInfo.GetTimingPointOrder(notExistTimingPoint);
        Assert.That(notExistTimingPointOrder, Is.Null);
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
        Assert.That(mappingTimingPoints.Length, Is.EqualTo(1));
        Assert.That(mappingTimingPoints[0].ID, Is.EqualTo(mappingTimingPoint.ID));
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
        Assert.That(result.Item1, Is.EqualTo(expectedStartTime));
        Assert.That(result.Item2, Is.EqualTo(expectedEndTime));
    }

    [Test]
    public void TestGetStartTime()
    {
        var timingInfo = new ClassicLyricTimingInfo();

        Assert.That(timingInfo.GetStartTime(), Is.Null);

        // Test add timing point.
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });

        Assert.That(timingInfo.GetStartTime(), Is.EqualTo(1000));
    }

    [Test]
    public void TestGetEndTime()
    {
        var timingInfo = new ClassicLyricTimingInfo();

        Assert.That(timingInfo.GetEndTime(), Is.Null);

        // Test add timing point.
        timingInfo.AddTimingPoint(x =>
        {
            x.Time = 1000;
        });

        Assert.That(timingInfo.GetEndTime(), Is.EqualTo(1000));
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
        Assert.That(mappingIds, Is.EqualTo(new[] { lyric.ID }));
    }

    #endregion
}
