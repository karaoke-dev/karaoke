// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Preview;
using osu.Game.Rulesets.Karaoke.Stages.Infos.Types;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

/// <summary>
/// Environment for execute the stage.
/// </summary>
public partial class DrawableStage : Container
{
    public DrawableStage()
    {
        RelativeSizeAxes = Axes.Both;
    }

    [BackgroundDependencyLoader]
    private void load(IReadOnlyList<Mod> mods, IBeatmap beatmap)
    {
        if (beatmap is not KaraokeBeatmap karaokeBeatmap)
            throw new InvalidOperationException();

        TriggerRecalculate(karaokeBeatmap, mods);
    }

    public void TriggerRecalculate(KaraokeBeatmap karaokeBeatmap, IReadOnlyList<Mod> mods)
    {
        var stageInfo = getStageInfo(mods, karaokeBeatmap);
        applyStageInfoToHitObject(stageInfo, karaokeBeatmap);
    }

    private static StageInfo getStageInfo(IReadOnlyList<Mod> mods, KaraokeBeatmap beatmap)
    {
        // todo: get all available stages from resource provider.
        var availableStageInfos = Array.Empty<StageInfo>();

        var stageMod = mods.OfType<ModStage>().SingleOrDefault();
        if (stageMod == null)
            return availableStageInfos.FirstOrDefault() ?? new PreviewStageInfo();

        var matchedStageInfo = availableStageInfos.FirstOrDefault(x => stageMod.IsStageInfoMatched(x));

        if (matchedStageInfo == null)
            matchedStageInfo = stageMod.GenerateDefaultStageInfo(beatmap);

        stageMod.ApplyToStageInfo(matchedStageInfo);
        return matchedStageInfo;
    }

    private static void applyStageInfoToHitObject(StageInfo stageInfo, KaraokeBeatmap beatmap)
    {
        // should invalidate the working property and fill it again.
        beatmap.HitObjects.OfType<Lyric>().ForEach(x =>
        {
            x.InvalidateWorkingProperty(LyricWorkingProperty.CommandGenerator);
        });
        beatmap.HitObjects.OfType<Note>().ForEach(x => x.InvalidateWorkingProperty(NoteWorkingProperty.CommandGenerator));

        // fill the working property.
        if (stageInfo is IHasCalculatedProperty calculatedProperty)
            calculatedProperty.ValidateCalculatedProperty(beatmap);
    }
}
