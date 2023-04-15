// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Preview;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages.Preview;

public class LyricPreviewStageEffectApplier : LyricStageEffectApplier<PreviewStageDefinition>
{
    public LyricPreviewStageEffectApplier(IEnumerable<StageElement> elements, PreviewStageDefinition definition)
        : base(elements, definition)
    {
    }

    protected override void UpdateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        switch (element)
        {
            case PreviewLyricLayout previewLyricLayout:
                updateInitialTransforms(transformSequence, previewLyricLayout);
                break;

            case PreviewStyle:
                // todo: should load the lyric style.
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void updateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, PreviewLyricLayout layout)
    {
        float initialPosition = getStartPosition(Definition, layout) + Definition.FadingOffsetPosition;
        float startPosition = getStartPosition(Definition, layout);
        float alpha = getAlpha(layout, Definition);
        double duration = fadeInDuration(layout, Definition);

        transformSequence.MoveToY(initialPosition).Then()
                         .MoveToY(startPosition, duration, Definition.MovingInEasing)
                         .FadeTo(alpha, duration, Definition.FadeInEasing);

        static float getStartPosition(PreviewStageDefinition definition, PreviewLyricLayout layout)
        {
            if (layout.StartTime != 0)
            {
                return definition.LyricHeight * (definition.NumberOfLyrics - 1);
            }

            return definition.LyricHeight * layout.Timings.Count;
        }

        static float getAlpha(PreviewLyricLayout layout, PreviewStageDefinition definition)
        {
            if (layout.Timings.Any())
            {
                return definition.InactiveAlpha;
            }

            return 1;
        }

        static double fadeInDuration(PreviewLyricLayout layout, PreviewStageDefinition definition)
        {
            if (layout.StartTime != 0)
            {
                return definition.FadingTime;
            }

            return 0;
        }
    }

    protected override void UpdateStartTimeStateTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        switch (element)
        {
            case PreviewLyricLayout previewLyricLayout:
                updateStartTimeStateTransforms(transformSequence, previewLyricLayout);
                break;

            case PreviewStyle:
                // todo: should load the lyric style.
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void updateStartTimeStateTransforms(TransformSequence<DrawableLyric> transformSequence, PreviewLyricLayout layout)
    {
        double relativeTime = layout.StartTime;

        foreach ((int line, double time) in layout.Timings)
        {
            double offsetTime = time - relativeTime;

            float position = Definition.LyricHeight * line;
            float alpha = line == 0 ? 1 : Definition.InactiveAlpha;
            double fadingTime = Math.Clamp(Definition.ActiveTime, 0, Definition.LineMovingTime);

            transformSequence.Delay(offsetTime)
                             .Then()
                             .MoveToY(position, Definition.LineMovingTime, Definition.LineMovingEasing)
                             .FadeTo(alpha, fadingTime, Definition.ActiveEasing)
                             .Then();

            relativeTime = relativeTime + offsetTime + Definition.LineMovingTime;
        }
    }

    protected override void UpdateHitStateTransforms(TransformSequence<DrawableLyric> transformSequence, ArmedState state, StageElement element)
    {
        switch (element)
        {
            case PreviewLyricLayout previewLyricLayout:
                updateHitStateTransforms(transformSequence, state, previewLyricLayout);
                break;

            case PreviewStyle:
                // todo: should load the lyric style.
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void updateHitStateTransforms(TransformSequence<DrawableLyric> transformSequence, ArmedState state, PreviewLyricLayout layout)
    {
        float targetPosition = -Definition.FadingOffsetPosition;

        transformSequence.FadeOut(Definition.FadingTime, Definition.FadeOutEasing)
                         .MoveToY(targetPosition, Definition.FadingTime, Definition.MoveOutEasing);
    }
}
