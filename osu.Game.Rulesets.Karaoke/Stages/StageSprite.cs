// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Commands;

namespace osu.Game.Rulesets.Karaoke.Stages;

public class StageSprite : IStageElementWithDuration
{
    public double StartTime { get; }

    public double EndTime { get; }

    public double EndTimeForDisplay { get; }

    public IReadOnlyList<IStageCommand> Commands { get; init; } = new List<IStageCommand>();

    public virtual Drawable CreateDrawable()
    {
        throw new System.NotImplementedException();
    }

    public void ApplyTransforms<TDrawable>(TDrawable drawable)
        where TDrawable : Drawable
    {
        HashSet<string> appliedProperties = new HashSet<string>();

        foreach (var command in Commands.OrderBy(c => c.StartTime))
        {
            if (appliedProperties.Add(command.PropertyName))
                command.ApplyInitialValue(drawable);

            using (drawable.BeginAbsoluteSequence(command.StartTime))
                command.ApplyTransforms(drawable);
        }
    }
}
