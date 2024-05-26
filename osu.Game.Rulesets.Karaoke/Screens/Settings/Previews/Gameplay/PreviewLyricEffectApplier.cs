// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Objects.Stages;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Settings.Previews.Gameplay;

/// <summary>
/// Effect applier for single lyric.
/// </summary>
public class PreviewLyricEffectApplier : LyricStageEffectApplier<PreviewLyricStageDefinition>
{
    public PreviewLyricEffectApplier()
        : base(new StageElement[] { new PreviewLyricStageElement() }, new PreviewLyricStageDefinition())
    {
    }

    protected override double GetPreemptTime(IEnumerable<StageElement> elements)
    {
        return 0;
    }

    protected override void UpdateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        switch (element)
        {
            case PreviewLyricStageElement previewLyricLayout:
                updateInitialTransforms(transformSequence, previewLyricLayout);
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void updateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, PreviewLyricStageElement element)
    {
        transformSequence.TransformTo(nameof(DrawableLyric.Anchor), Anchor.Centre)
                         .TransformTo(nameof(DrawableLyric.Origin), Anchor.Centre)
                         .TransformTo(nameof(DrawableLyric.Scale), new Vector2(2))
                         .FadeIn(100);
    }

    protected override void UpdateStartTimeStateTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        // there's no transformer in here.
    }

    protected override void UpdateHitStateTransforms(TransformSequence<DrawableLyric> transformSequence, ArmedState state, StageElement element)
    {
        transformSequence.FadeOut(100);
    }
}
