// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Stages.Classic;

public abstract partial class ClassicStageScreenTestScene<T> : GenericEditorScreenTestScene<T, ClassicStageEditorScreenMode>
    where T : ClassicStageScreen
{
    protected override KaraokeBeatmap CreateBeatmap()
    {
        var karaokeBeatmap = base.CreateBeatmap();

        // add classic stage info for testing purpose.
        karaokeBeatmap.StageInfos.Add(new ClassicStageInfo());

        return karaokeBeatmap;
    }
}
