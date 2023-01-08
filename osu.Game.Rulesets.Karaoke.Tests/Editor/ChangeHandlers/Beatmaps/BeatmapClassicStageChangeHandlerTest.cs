// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapClassicStageChangeHandlerTest : BaseChangeHandlerTest<BeatmapClassicStageChangeHandler>
{
    #region Layout definition

    [Test]
    public void TestEditLayoutDefinition()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.StageInfos.Add(new ClassicStageInfo());
        });

        TriggerHandlerChanged(c =>
        {
            c.EditLayoutDefinition(x =>
            {
                x.LineHeight = 12;
            });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var classicStageInfo = getClassicStageInfo(karaokeBeatmap);
            Assert.IsNotNull(classicStageInfo);

            var definition = classicStageInfo.LyricLayoutDefinition;
            Assert.AreEqual(12, definition.LineHeight);
        });
    }

    #endregion

    #region Timing info

    [Test]
    public void TestAddTimingPoint()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.StageInfos.Add(new ClassicStageInfo());
        });

        TriggerHandlerChanged(c =>
        {
            c.AddTimingPoint(new ClassicLyricTimingPoint
            {
                Time = 1000
            });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            var timingPoint = timingInfo.Timings.FirstOrDefault();
            Assert.IsNotNull(timingPoint);
            Assert.AreEqual(1000, timingPoint!.Time);
        });
    }

    [Test]
    public void TestRemoveTimingPoint()
    {
        ClassicLyricTimingPoint removedTimingPoint = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var classicStageInfo = new ClassicStageInfo();
            classicStageInfo.LyricTimingInfo.Timings.Add(new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            classicStageInfo.LyricTimingInfo.Timings.Add(removedTimingPoint = new ClassicLyricTimingPoint
            {
                Time = 2000,
            });

            karaokeBeatmap.StageInfos.Add(classicStageInfo);
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveTimingPoint(removedTimingPoint);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            var timingPoint = timingInfo.Timings.FirstOrDefault();
            Assert.IsNotNull(timingPoint);
            Assert.AreEqual(1000, timingPoint!.Time);
        });
    }

    [Test]
    public void TestRemoveRangeOfTimingPoints()
    {
        ClassicLyricTimingPoint removedTimingPoint = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var classicStageInfo = new ClassicStageInfo();
            classicStageInfo.LyricTimingInfo.Timings.Add(new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            classicStageInfo.LyricTimingInfo.Timings.Add(removedTimingPoint = new ClassicLyricTimingPoint
            {
                Time = 2000,
            });

            karaokeBeatmap.StageInfos.Add(classicStageInfo);
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveRangeOfTimingPoints(new[] { removedTimingPoint });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            var timingPoint = timingInfo.Timings.FirstOrDefault();
            Assert.IsNotNull(timingPoint);
            Assert.AreEqual(1000, timingPoint!.Time);
        });
    }

    [Test]
    public void TestShiftingTimingPoints()
    {
        ClassicLyricTimingPoint shiftingTimingPoint1 = null!;
        ClassicLyricTimingPoint shiftingTimingPoint2 = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var classicStageInfo = new ClassicStageInfo();
            classicStageInfo.LyricTimingInfo.Timings.Add(shiftingTimingPoint1 = new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            classicStageInfo.LyricTimingInfo.Timings.Add(shiftingTimingPoint2 = new ClassicLyricTimingPoint
            {
                Time = 2000,
            });

            karaokeBeatmap.StageInfos.Add(classicStageInfo);
        });

        TriggerHandlerChanged(c =>
        {
            c.ShiftingTimingPoints(new[] { shiftingTimingPoint1, shiftingTimingPoint2 }, 100);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            var timingPoint = timingInfo.Timings;
            Assert.AreEqual(2, timingPoint.Count);
            Assert.AreEqual(1100, timingPoint[0].Time);
            Assert.AreEqual(2100, timingPoint[1].Time);
        });
    }

    [Test]
    public void TestAddLyricIntoTimingPoint()
    {
        ClassicLyricTimingPoint timingPoint = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var classicStageInfo = new ClassicStageInfo();
            classicStageInfo.LyricTimingInfo.Timings.Add(timingPoint = new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            karaokeBeatmap.StageInfos.Add(classicStageInfo);
        });

        PrepareHitObject(new Lyric { ID = 1 });
        PrepareHitObject(new Lyric { ID = 2 }, false);

        TriggerHandlerChanged(c =>
        {
            c.AddLyricIntoTimingPoint(timingPoint);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            var actualTimingPoint = timingInfo.Timings.First();
            Assert.AreEqual(new[] { 1 }, actualTimingPoint!.LyricIds);
        });
    }

    [Test]
    public void TestRemoveLyricFromTimingPoint()
    {
        ClassicLyricTimingPoint timingPoint = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var classicStageInfo = new ClassicStageInfo();
            classicStageInfo.LyricTimingInfo.Timings.Add(timingPoint = new ClassicLyricTimingPoint
            {
                Time = 1000,
                LyricIds = new[] { 1, 2 }
            });

            karaokeBeatmap.StageInfos.Add(classicStageInfo);
        });

        PrepareHitObject(new Lyric { ID = 1 });
        PrepareHitObject(new Lyric { ID = 2 }, false);

        TriggerHandlerChanged(c =>
        {
            c.RemoveLyricFromTimingPoint(timingPoint);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            var actualTimingPoint = timingInfo.Timings.First();
            Assert.AreEqual(new[] { 2 }, actualTimingPoint!.LyricIds);
        });
    }

    #endregion

    private static ClassicStageInfo getClassicStageInfo(KaraokeBeatmap karaokeBeatmap)
        => karaokeBeatmap.StageInfos.OfType<ClassicStageInfo>().First();
}
