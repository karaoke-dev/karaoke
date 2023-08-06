// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Stages.Classic;

public partial class TestSceneClassicStageEditor : GenericEditorTestScene<ClassicStageEditor, ClassicStageEditorScreenMode>
{
    protected override KaraokeBeatmap CreateBeatmap()
    {
        var karaokeBeatmap = base.CreateBeatmap();

        // add classic stage info for testing purpose.
        karaokeBeatmap.StageInfos.Add(new ClassicStageInfo());

        return karaokeBeatmap;
    }
}
