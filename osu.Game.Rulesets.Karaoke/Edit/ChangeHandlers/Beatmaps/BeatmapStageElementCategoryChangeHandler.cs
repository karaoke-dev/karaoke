// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public partial class BeatmapStageElementCategoryChangeHandler<TStageElement, THitObject> : BeatmapPropertyChangeHandler, IBeatmapStageElementCategoryChangeHandler<TStageElement, THitObject>
    where TStageElement : class, IStageElement
    where THitObject : KaraokeHitObject, IHasPrimaryKey
{
    private readonly Func<IEnumerable<StageInfo>, StageElementCategory<TStageElement, THitObject>> action;

    public BeatmapStageElementCategoryChangeHandler(Func<IEnumerable<StageInfo>, StageElementCategory<TStageElement, THitObject>> action)
    {
        this.action = action;
    }

    public void AddElement(Action<TStageElement>? action = null)
    {
        performStageInfoChanged(s =>
        {
            s.AddElement(action);
        });
    }

    public void EditElement(int? id, Action<TStageElement> action)
    {
        performStageInfoChanged(s =>
        {
            s.EditElement(id, action);
        });
    }

    public void RemoveElement(TStageElement element)
    {
        performStageInfoChanged(s =>
        {
            s.RemoveElement(element);
        });
    }

    public void AddToMapping(TStageElement element, THitObject hitObject)
    {
        performStageInfoChanged(s =>
        {
            s.AddToMapping(element, hitObject);
        });
    }

    public void RemoveFromMapping(THitObject hitObject)
    {
        performStageInfoChanged(s =>
        {
            s.RemoveFromMapping(hitObject);
        });
    }

    public void ClearUnusedMapping()
    {
        performStageInfoChanged(s =>
        {
            s.ClearUnusedMapping(id => Lyrics.Any(x => x.ID == id));
        });
    }

    private void performStageInfoChanged(Action<StageElementCategory<TStageElement, THitObject>> stageAction)
    {
        PerformBeatmapChanged(beatmap =>
        {
            var stageCategory = action(beatmap.StageInfos);
            stageAction(stageCategory);
        });
    }
}
