// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapClassicStageChangeHandlerTest : BaseChangeHandlerTest<BeatmapClassicStageChangeHandler>
{
    #region Layout definition

    [Test]
    public void TestEditLayoutDefinition()
    {
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

            // assert definition.
            var definition = classicStageInfo.StageDefinition;
            Assert.AreEqual(12, definition.LineHeight);
        });
    }

    #endregion

    #region Timing info

    [Test]
    public void TestAddTimingPoint()
    {
        TriggerHandlerChanged(c =>
        {
            c.AddTimingPoint(x =>
            {
                x.Time = 1000;
            });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            Assert.IsNotNull(timingInfo);

            // assert timing.
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
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            timingInfo.Timings.Add(new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            timingInfo.Timings.Add(removedTimingPoint = new ClassicLyricTimingPoint
            {
                Time = 2000,
            });
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveTimingPoint(removedTimingPoint);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            Assert.IsNotNull(timingInfo);

            // assert timing.
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
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            timingInfo.Timings.Add(new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            timingInfo.Timings.Add(removedTimingPoint = new ClassicLyricTimingPoint
            {
                Time = 2000,
            });
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveRangeOfTimingPoints(new[] { removedTimingPoint });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            Assert.IsNotNull(timingInfo);

            // assert timing.
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
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            timingInfo.Timings.Add(shiftingTimingPoint1 = new ClassicLyricTimingPoint
            {
                Time = 1000,
            });

            timingInfo.Timings.Add(shiftingTimingPoint2 = new ClassicLyricTimingPoint
            {
                Time = 2000,
            });
        });

        TriggerHandlerChanged(c =>
        {
            c.ShiftingTimingPoints(new[] { shiftingTimingPoint1, shiftingTimingPoint2 }, 100);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            Assert.IsNotNull(timingInfo);

            // assert timing.
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
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            timingInfo.Timings.Add(timingPoint = new ClassicLyricTimingPoint
            {
                Time = 1000,
            });
        });

        Lyric lyric1 = null!;
        Lyric lyric2 = null!;

        PrepareHitObject(() => lyric1 = new Lyric());
        PrepareHitObject(() => lyric2 = new Lyric(), false);

        TriggerHandlerChanged(c =>
        {
            c.AddLyricIntoTimingPoint(timingPoint);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            Assert.IsNotNull(timingInfo);

            // assert mapping status.
            Assert.AreEqual(new[] { timingPoint }, timingInfo.GetLyricTimingPoints(lyric1));
            Assert.IsEmpty(timingInfo.GetLyricTimingPoints(lyric2));
        });
    }

    [Test]
    public void TestRemoveLyricFromTimingPoint()
    {
        ClassicLyricTimingPoint timingPoint = null!;

        Lyric lyric1 = null!;
        Lyric lyric2 = null!;

        PrepareHitObject(() => lyric1 = new Lyric());
        PrepareHitObject(() => lyric2 = new Lyric(), false);

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            timingInfo.Timings.Add(timingPoint = new ClassicLyricTimingPoint
            {
                Time = 1000,
            });
            timingInfo.AddToMapping(timingPoint, lyric1);
            timingInfo.AddToMapping(timingPoint, lyric2);
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveLyricFromTimingPoint(timingPoint);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var timingInfo = getLyricTimingInfo(karaokeBeatmap);
            Assert.IsNotNull(timingInfo);

            // assert mapping status.
            Assert.IsEmpty(timingInfo.GetLyricTimingPoints(lyric1)); // should clear the mapping in the lyric1 because it's being selected.
            Assert.AreEqual(new[] { timingPoint }, timingInfo.GetLyricTimingPoints(lyric2));
        });
    }

    #endregion

    protected override void SetUpKaraokeBeatmap(Action<KaraokeBeatmap> action)
    {
        base.SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            karaokeBeatmap.StageInfos = new List<StageInfo>
            {
                stageInfo,
            };
            karaokeBeatmap.CurrentStageInfo = stageInfo;

            action(karaokeBeatmap);
        });
    }

    private static ClassicStageInfo getClassicStageInfo(KaraokeBeatmap karaokeBeatmap)
        => karaokeBeatmap.StageInfos.OfType<ClassicStageInfo>().First();

    private static ClassicLyricTimingInfo getLyricTimingInfo(KaraokeBeatmap karaokeBeatmap)
        => getClassicStageInfo(karaokeBeatmap).LyricTimingInfo;
}
