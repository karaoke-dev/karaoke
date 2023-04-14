// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages;

public abstract class LyricStageEffectApplier : StageEffectApplier<DrawableLyric>
{
    protected LyricStageEffectApplier(IEnumerable<StageElement> elements, StageDefinition definition)
        : base(elements, definition)
    {
    }
}
