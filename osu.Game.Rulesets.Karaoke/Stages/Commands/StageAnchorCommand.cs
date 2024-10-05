// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands;

public class StageAnchorCommand : StageCommand<Anchor>
{
    public StageAnchorCommand(Easing easing, double startTime, double endTime, Anchor startValue, Anchor endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => nameof(Drawable.Anchor);

    public override void ApplyInitialValue<TDrawable>(TDrawable d)
    {
        if (StartTime == EndTime)
            d.Anchor = StartValue;
    }
    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
        => d.TransformTo(nameof(Drawable.Anchor), StartValue).Delay(Duration)
            .TransformTo(nameof(Drawable.Anchor), EndValue);
}
