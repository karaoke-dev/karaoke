// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;

public class LyricClassicStageEffectApplier : LyricStageEffectApplier<ClassicStageDefinition>
{
    public LyricClassicStageEffectApplier(IEnumerable<StageElement> elements, ClassicStageDefinition definition)
        : base(elements, definition)
    {
    }

    protected override void UpdateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateStartTimeStateTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateHitStateTransforms(TransformSequence<DrawableLyric> transformSequence, ArmedState state, StageElement element)
    {
        throw new System.NotImplementedException();
    }
}
