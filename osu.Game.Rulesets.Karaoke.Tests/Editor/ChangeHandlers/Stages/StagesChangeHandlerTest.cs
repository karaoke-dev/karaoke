// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Stages;

[Ignore("Ignore all stage-related change handler test until able to edit the stage info.")]
public partial class StagesChangeHandlerTest : BaseStageInfoChangeHandlerTest<StagesChangeHandler>
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

        AssertStageInfos(stageInfos =>
        {
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
        SetUpStageInfo<ClassicStageInfo>();

        TriggerHandlerChanged(c =>
        {
            c.Remove<ClassicStageInfo>();
        });

        AssertStageInfos(stageInfos =>
        {
            Assert.AreEqual(0, stageInfos.Count);
        });

        // Should not remove if there's no matched stage info type.
        TriggerHandlerChangedWithException<InvalidOperationException>(c =>
        {
            c.Remove<ClassicStageInfo>();
        });
    }
}
