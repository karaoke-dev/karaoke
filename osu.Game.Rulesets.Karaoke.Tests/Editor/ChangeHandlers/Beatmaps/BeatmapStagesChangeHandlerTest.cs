// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapStagesChangeHandlerTest : BaseChangeHandlerTest<BeatmapStagesChangeHandler>
{
    protected override bool IncludeAutoGenerator => true;

    [Test]
    public void TestAutoGenerate()
    {
        PrepareHitObject(() => new Lyric(), false);
        PrepareHitObject(() => new Lyric(), false);

        TriggerHandlerChanged(c =>
        {
            c.AutoGenerate<ClassicStageInfo>();
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfos = karaokeBeatmap.StageInfos;
            Assert.AreEqual(1, stageInfos.Count);
            Assert.AreEqual(typeof(ClassicStageInfo), stageInfos[0].GetType());
        });

        // Should not add the same stage again.
        TriggerHandlerChangedWithException<InvalidOperationException>(c =>
        {
            c.AutoGenerate<ClassicStageInfo>();
        });
    }

    [Test]
    public void TestRemove()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfo = new ClassicStageInfo();
            karaokeBeatmap.StageInfos = new List<StageInfo>
            {
                stageInfo,
            };
            karaokeBeatmap.CurrentStageInfo = stageInfo;
        });

        TriggerHandlerChanged(c =>
        {
            c.Remove<ClassicStageInfo>();
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var stageInfos = karaokeBeatmap.StageInfos;
            Assert.AreEqual(0, stageInfos.Count);
        });

        // Should not remove if there's no matched stage info type.
        TriggerHandlerChangedWithException<InvalidOperationException>(c =>
        {
            c.Remove<ClassicStageInfo>();
        });
    }
}
