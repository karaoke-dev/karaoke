// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Stages;

[Ignore("Ignore all stage-related change handler test until able to edit the stage info.")]
public partial class ClassicStageChangeHandlerTest : BaseStageInfoChangeHandlerTest<ClassicStageChangeHandler>
{
    #region Layout definition

    [Test]
    public void TestEditLayoutDefinition()
    {
        SetUpStageInfo<ClassicStageInfo>();

        TriggerHandlerChanged(c =>
        {
            c.EditLayoutDefinition(x =>
            {
                x.LineHeight = 12;
            });
        });

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            Assert.IsNotNull(stageInfo);

            // assert definition.
            var definition = stageInfo.StageDefinition;
            Assert.AreEqual(12, definition.LineHeight);
        });
    }

    #endregion

    #region Timing info

    [Test]
    public void TestAddTimingPoint()
    {
        SetUpStageInfo<ClassicStageInfo>();

        TriggerHandlerChanged(c =>
        {
            c.AddTimingPoint(x =>
            {
                x.Time = 1000;
            });
        });

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        SetUpStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        SetUpStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        SetUpStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        SetUpStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        SetUpStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
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

        AssertStageInfo<ClassicStageInfo>(stageInfo =>
        {
            var timingInfo = stageInfo.LyricTimingInfo;
            Assert.IsNotNull(timingInfo);

            // assert mapping status.
            Assert.IsEmpty(timingInfo.GetLyricTimingPoints(lyric1)); // should clear the mapping in the lyric1 because it's being selected.
            Assert.AreEqual(new[] { timingPoint }, timingInfo.GetLyricTimingPoints(lyric2));
        });
    }

    #endregion
}
