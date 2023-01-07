// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

public interface IBeatmapStageElementCategoryChangeHandler<TStageElement, in THitObject>
    where TStageElement : IStageElement
    where THitObject : KaraokeHitObject, IHasPrimaryKey
{
    void AddElement(Action<TStageElement>? action = null);

    void EditElement(int? id, Action<TStageElement> action);

    void RemoveElement(TStageElement element);

    void AddToMapping(TStageElement element, THitObject hitObject);

    void RemoveFromMapping(THitObject hitObject);

    void ClearUnusedMapping();
}
