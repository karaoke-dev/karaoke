// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands;

public class StageOriginCommand : StageCommand<Anchor>
{
    public StageOriginCommand(Easing easing, double startTime, double endTime, Anchor startValue, Anchor endValue)
        : base(easing, startTime, endTime, startValue, endValue)
    {
    }

    public override string PropertyName => nameof(Drawable.Origin);

    public override void ApplyInitialValue<TDrawable>(TDrawable d)
    {
        if (StartTime == EndTime)
            d.Origin = StartValue;
    }
    public override TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
        => d.TransformTo(nameof(Drawable.Origin), StartValue).Delay(Duration)
            .TransformTo(nameof(Drawable.Origin), EndValue);
}
