// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages.Preview;

public class NotePreviewStageEffectApplier : NoteStageEffectApplier<PreviewStageDefinition>
{
    public NotePreviewStageEffectApplier(IEnumerable<StageElement> elements, PreviewStageDefinition definition)
        : base(elements, definition)
    {
    }

    protected override double GetPreemptTime(IEnumerable<StageElement> elements)
    {
        // todo: implementation needed.
        return 0;
    }

    protected override void UpdateInitialTransforms(TransformSequence<DrawableNote> transformSequence, StageElement element)
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateStartTimeStateTransforms(TransformSequence<DrawableNote> transformSequence, StageElement element)
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdateHitStateTransforms(TransformSequence<DrawableNote> transformSequence, ArmedState state, StageElement element)
    {
        throw new System.NotImplementedException();
    }
}
