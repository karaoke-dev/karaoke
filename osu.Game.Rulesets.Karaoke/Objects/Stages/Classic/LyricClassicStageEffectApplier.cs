// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Stages;
using osu.Game.Rulesets.Karaoke.Stages.Classic;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages.Classic;

public class LyricClassicStageEffectApplier : LyricStageEffectApplier<ClassicStageDefinition>
{
    public LyricClassicStageEffectApplier(IEnumerable<StageElement> elements, ClassicStageDefinition definition)
        : base(elements, definition)
    {
    }

    protected override double GetPreemptTime(IEnumerable<StageElement> elements)
    {
        return Definition.FadeInTime;
    }

    protected override void UpdateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        switch (element)
        {
            case ClassicLyricLayout previewLyricLayout:
                updateInitialTransforms(transformSequence, previewLyricLayout);
                break;

            case ClassicStyle:
                // todo: should load the lyric style.
                break;

            default:
                throw new NotSupportedException();
        }
    }

    private void updateInitialTransforms(TransformSequence<DrawableLyric> transformSequence, ClassicLyricLayout layout)
    {
        transformSequence.TransformTo(nameof(DrawableLyric.Anchor), getLyricAnchor(layout.Alignment))
                         .TransformTo(nameof(DrawableLyric.Origin), getLyricAnchor(layout.Alignment))
                         .TransformTo(nameof(DrawableLyric.Padding), getPosition(Definition, layout))
                         .TransformTo(nameof(DrawableLyric.Scale), new Vector2(Definition.LyricScale))
                         .FadeTo(1, Definition.FadeInTime, Definition.FadeInEasing);

        static Anchor getLyricAnchor(ClassicLyricLayoutAlignment alignment) =>
            alignment switch
            {
                ClassicLyricLayoutAlignment.Left => Anchor.BottomLeft,
                ClassicLyricLayoutAlignment.Center => Anchor.BottomCentre,
                ClassicLyricLayoutAlignment.Right => Anchor.BottomRight,
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null),
            };

        static MarginPadding getPosition(ClassicStageDefinition definition, ClassicLyricLayout layout)
        {
            float paddingX = definition.BorderWidth + layout.HorizontalMargin;
            float paddingY = definition.BorderHeight + definition.LineHeight * layout.Line;

            var padding = new MarginPadding
            {
                Bottom = paddingY,
            };

            switch (layout.Alignment)
            {
                case ClassicLyricLayoutAlignment.Left:
                    padding.Left = paddingX;
                    return padding;

                case ClassicLyricLayoutAlignment.Center:
                    return padding;

                case ClassicLyricLayoutAlignment.Right:
                    padding.Right = paddingX;
                    return padding;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    protected override void UpdateStartTimeStateTransforms(TransformSequence<DrawableLyric> transformSequence, StageElement element)
    {
        // there's no transformer in here.
    }

    protected override void UpdateHitStateTransforms(TransformSequence<DrawableLyric> transformSequence, ArmedState state, StageElement element)
    {
        transformSequence.FadeOut(Definition.FadeOutTime, Definition.FadeOutEasing);
    }
}
