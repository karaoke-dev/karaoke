// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapClassicStageChangeHandlerTest : BaseChangeHandlerTest<BeatmapClassicStageChangeHandler>
{
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
            var classicStageInfo = karaokeBeatmap.StageInfos.OfType<ClassicStageInfo>().First();
            Assert.IsNotNull(classicStageInfo);

            var definition = classicStageInfo.LyricLayoutDefinition;
            Assert.AreEqual(12, definition.LineHeight);
        });
    }
}
