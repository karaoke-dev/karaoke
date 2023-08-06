// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Stages;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages;

public abstract class LyricStageEffectApplier<TStageDefinition> : StageEffectApplier<TStageDefinition, DrawableLyric>
    where TStageDefinition : StageDefinition
{
    protected LyricStageEffectApplier(IEnumerable<StageElement> elements, TStageDefinition definition)
        : base(elements, definition)
    {
    }
}
