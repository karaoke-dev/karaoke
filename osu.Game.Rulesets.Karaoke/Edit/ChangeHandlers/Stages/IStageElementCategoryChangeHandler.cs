// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Stages.Infos;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Stages;

public interface IStageElementCategoryChangeHandler<TStageElement>
    where TStageElement : StageElement
{
    void AddElement(Action<TStageElement>? action = null);

    void EditElement(ElementId? id, Action<TStageElement> action);

    void RemoveElement(TStageElement element);

    void AddToMapping(TStageElement element);

    void OffsetMapping(int offset);

    void RemoveFromMapping();

    void ClearUnusedMapping();
}
