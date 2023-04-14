// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages;

public abstract class StageEffectApplier<TStageDefinition, TDrawableHitObject> : IStageEffectApplier
    where TStageDefinition : StageDefinition
    where TDrawableHitObject : DrawableHitObject
{
    private readonly IEnumerable<StageElement> elements;
    private readonly TStageDefinition definition;

    protected StageEffectApplier(IEnumerable<StageElement> elements, TStageDefinition definition)
    {
        this.elements = elements;
        this.definition = definition;
    }
}
