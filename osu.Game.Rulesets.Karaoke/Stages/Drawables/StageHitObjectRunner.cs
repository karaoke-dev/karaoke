// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public partial class StageHitObjectRunner : Component, IStageHitObjectRunner
{
    public event Action? OnStageChanged;

    public event Action? OnCommandUpdated;

    private IHitObjectCommandGenerator commandGenerator = null!;

    public void UpdateCommandGenerator(IHitObjectCommandGenerator generator)
    {
        commandGenerator = generator;
        OnStageChanged?.Invoke();
    }

    public void TriggerUpdateCommand()
    {
        OnCommandUpdated?.Invoke();
    }

    public double GetPreemptTime(HitObject hitObject)
    {
        return commandGenerator.GeneratePreemptTime(hitObject);
    }

    public double GetStartTimeOffset(HitObject hitObject)
    {
        return commandGenerator.GenerateStartTimeOffset(hitObject);
    }

    public double GetEndTimeOffset(HitObject hitObject)
    {
        return commandGenerator.GenerateEndTimeOffset(hitObject);
    }

    public void UpdateInitialTransforms(DrawableHitObject drawableHitObject)
    {
        var commands = commandGenerator.GenerateInitialCommands(drawableHitObject.HitObject);
        ApplyTransforms(drawableHitObject, commands);
    }

    public void UpdateStartTimeStateTransforms(DrawableHitObject drawableHitObject)
    {
        var commands = commandGenerator.GenerateStartTimeStateCommands(drawableHitObject.HitObject);
        double startTimeOffset = -commandGenerator.GenerateStartTimeOffset(drawableHitObject.HitObject);
        ApplyTransforms(drawableHitObject, commands, startTimeOffset);
    }

    public void UpdateHitStateTransforms(DrawableHitObject drawableHitObject, ArmedState state)
    {
        if (state == ArmedState.Idle)
            return;

        var commands = commandGenerator.GenerateHitStateCommands(drawableHitObject.HitObject, state);
        ApplyTransforms(drawableHitObject, commands);
    }

    public static void ApplyTransforms<TDrawable>(TDrawable drawable, IEnumerable<IStageCommand> commands, double offset = 0)
        where TDrawable : DrawableHitObject
    {
        var appliedProperties = new HashSet<string>();

        foreach (var command in commands.OrderBy(c => c.StartTime))
        {
            if (appliedProperties.Add(command.PropertyName))
                command.ApplyInitialValue(drawable);

            using (drawable.BeginDelayedSequence(command.StartTime + offset))
                command.ApplyTransforms(drawable);
        }
    }
}
