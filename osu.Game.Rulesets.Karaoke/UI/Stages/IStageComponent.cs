// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Transforms;

namespace osu.Game.Rulesets.Karaoke.UI.Stages;

public interface IStageComponent : IDrawable;

internal class TransformAddStageComponent<TStageComponent> : Transform<TStageComponent, IAcceptStageComponent>
    where TStageComponent : class, IStageComponent
{
    public override string TargetMember => nameof(IAcceptStageComponent.Add);

    private bool added;

    protected override void Apply(IAcceptStageComponent d, double time)
    {
        if (time >= StartTime && !added)
        {
            d.Add(EndValue);
            added = true;
        }
        else if (time <= StartTime && added)
        {
            d.Remove(EndValue);
            added = false;
        }
    }

    protected override void ReadIntoStartValue(IAcceptStageComponent d) => StartValue = null!;
}

internal static class AcceptStageComponentExtensions
{
    /// <summary>
    /// Add the <see cref="IStageComponent"/> into the drawable which implement the <see cref="IAcceptStageComponent"/>
    /// </summary>
    /// <returns>A <see cref="TransformSequence{T}"/> to which further transforms can be added.</returns>
    public static TransformSequence<T> TransformAddStageComponent<T, TStageComponent>(this TransformSequence<T> t, TStageComponent component, double duration = 0)
        where T : class, IAcceptStageComponent
        where TStageComponent : class, IStageComponent
        => t.Append(o => o.TransformAddStageComponent(component, duration));

    /// <summary>
    /// Add the <see cref="IStageComponent"/> into the drawable which implement the <see cref="IAcceptStageComponent"/>
    /// </summary>
    /// <returns>A <see cref="TransformSequence{T}"/> to which further transforms can be added.</returns>
    public static TransformSequence<T> TransformAddStageComponent<T, TStageComponent>(this T acceptStageComponent, TStageComponent component, double duration = 0)
        where T : class, IAcceptStageComponent
        where TStageComponent : class, IStageComponent
        => acceptStageComponent.TransformTo(acceptStageComponent.PopulateTransform(new TransformAddStageComponent<TStageComponent>(), component, duration));
}
