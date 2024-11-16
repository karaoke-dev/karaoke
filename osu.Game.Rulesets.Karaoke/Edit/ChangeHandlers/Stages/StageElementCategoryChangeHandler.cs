// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;

public partial class StageElementCategoryChangeHandler<TStageElement, THitObject> : BeatmapPropertyChangeHandler, IStageElementCategoryChangeHandler<TStageElement>
    where TStageElement : StageElement, IComparable<TStageElement>, new()
    where THitObject : KaraokeHitObject, IHasPrimaryKey
{
    private readonly Func<IEnumerable<StageInfo>, StageElementCategory<TStageElement, THitObject>> stageCategoryAction;

    public StageElementCategoryChangeHandler(Func<IEnumerable<StageInfo>, StageElementCategory<TStageElement, THitObject>> stageCategoryAction)
    {
        this.stageCategoryAction = stageCategoryAction;
    }

    public void AddElement(Action<TStageElement>? action = null)
    {
        performStageInfoChanged(s =>
        {
            s.AddElement(action);
        });
    }

    public void EditElement(ElementId? id, Action<TStageElement> action)
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

    public void AddToMapping(TStageElement element)
    {
        PerformOnSelection<THitObject>(hitObject =>
        {
            performStageInfoChanged(s =>
            {
                s.AddToMapping(element, hitObject);
            });
        });
    }

    public void OffsetMapping(int offset)
    {
        if (offset == 0)
            throw new InvalidOperationException("Offset number should not be zero.");

        PerformOnSelection<THitObject>(hitObject =>
        {
            performStageInfoChanged(s =>
            {
                var element = s.GetElementByItem(hitObject);
                int mappingIndex = s.SortedElements.IndexOf(element);
                if (mappingIndex < 0)
                    return;

                int newMappingIndex = mappingIndex + offset;
                var newElement = s.SortedElements.ElementAtOrDefault(newMappingIndex);
                if (newElement == null)
                    return;

                s.RemoveHitObjectFromMapping(hitObject);
                s.AddToMapping(newElement, hitObject);
            });
        });
    }

    public void RemoveFromMapping()
    {
        PerformOnSelection<THitObject>(hitObject =>
        {
            performStageInfoChanged(s =>
            {
                s.RemoveHitObjectFromMapping(hitObject);
            });
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
            var stageCategory = stageCategoryAction(beatmap.StageInfos);
            stageAction(stageCategory);
        });
    }
}
