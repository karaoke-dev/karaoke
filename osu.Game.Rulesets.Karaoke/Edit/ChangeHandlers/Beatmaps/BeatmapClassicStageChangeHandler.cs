// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapClassicStageChangeHandler : BeatmapPropertyChangeHandler, IBeatmapClassicStageChangeHandler
{
    public void EditLayoutDefinition(Action<ClassicLyricLayoutDefinition> action)
    {
        performStageInfoChanged(x =>
        {
            action(x.LyricLayoutDefinition);
        });
    }

    private void performStageInfoChanged(Action<ClassicStageInfo> action)
    {
        PerformBeatmapChanged(beatmap =>
        {
            var stage = getStageInfo(beatmap);
            action(stage);
        });

        ClassicStageInfo getStageInfo(KaraokeBeatmap beatmap)
            => beatmap.StageInfos.OfType<ClassicStageInfo>().First();
    }
}
