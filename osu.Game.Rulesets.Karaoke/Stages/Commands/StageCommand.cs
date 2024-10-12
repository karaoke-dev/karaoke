// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Karaoke.Stages.Commands;

public abstract class StageCommand<T> : IStageCommand, IComparable<StageCommand<T>>
{
    public double StartTime { get; }
    public double EndTime { get; }

    public T StartValue { get; }
    public T EndValue { get; }
    public Easing Easing { get; }

    public double Duration => EndTime - StartTime;

    protected StageCommand(Easing easing, double startTime, double endTime, T startValue, T endValue)
    {
        if (endTime < startTime)
            endTime = startTime;

        StartTime = startTime;
        StartValue = startValue;
        EndTime = endTime;
        EndValue = endValue;
        Easing = easing;
    }

    public abstract string PropertyName { get; }

    public abstract void ApplyInitialValue<TDrawable>(TDrawable d)
        where TDrawable : Drawable;

    public abstract TransformSequence<TDrawable> ApplyTransforms<TDrawable>(TDrawable d)
        where TDrawable : Drawable;

    public int CompareTo(StageCommand<T>? other)
    {
        if (other == null)
            return 1;

        int result = StartTime.CompareTo(other.StartTime);
        if (result != 0)
            return result;

        return EndTime.CompareTo(other.EndTime);
    }

    public override string ToString() => $"{StartTime} -> {EndTime}, {StartValue} -> {EndValue} {Easing}";
}
