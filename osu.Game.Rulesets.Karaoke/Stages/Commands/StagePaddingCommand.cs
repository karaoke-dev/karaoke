// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands;

public class StagePaddingCommand : StageCommand<MarginPadding>
{
    public StagePaddingCommand(Easing easing, double startTime, double endTime, MarginPadding startValue, MarginPadding endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => nameof(CompositeDrawable.Padding);

    public override void ApplyInitialValue<TDrawable>(TDrawable d)
    {
        // todo: composite drawable can only change the padding by transform.
    }
    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
        => d.TransformTo(nameof(CompositeDrawable.Padding), StartValue).Delay(Duration)
            .TransformTo(nameof(CompositeDrawable.Padding), EndValue);
}
