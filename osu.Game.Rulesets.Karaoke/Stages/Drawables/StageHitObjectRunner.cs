// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public class StageHitObjectRunner : StageRunner, IStageHitObjectRunner
{
    public event Action? OnCommandUpdated;

    private IHitObjectCommandProvider commandProvider = null!;
    private IList<IApplicableToStageHitObjectCommand> stageMods = null!;

    public override void OnStageInfoChanged(StageInfo stageInfo, bool scorable, IReadOnlyList<Mod> mods)
    {
        commandProvider = stageInfo.CreateHitObjectCommandProvider<Lyric>()!;
        stageMods = mods.OfType<IApplicableToStageHitObjectCommand>().Where(x => x.CanApply(stageInfo)).ToList();

        OnCommandUpdated?.Invoke();
    }

    public override void TriggerUpdateCommand()
    {
        OnCommandUpdated?.Invoke();
    }

    public double GetPreemptTime(HitObject hitObject)
    {
        return commandProvider.GetPreemptTime(hitObject);
    }

    public double GetStartTimeOffset(HitObject hitObject)
    {
        return commandProvider.GetStartTimeOffset(hitObject);
    }

    public double GetEndTimeOffset(HitObject hitObject)
    {
        return commandProvider.GetEndTimeOffset(hitObject);
    }

    public void UpdateInitialTransforms(DrawableHitObject drawableHitObject)
    {
        var commands = commandProvider.GetInitialCommands(drawableHitObject.HitObject);

        commands = postProcessCommand(drawableHitObject.HitObject, commands, x => x.PostProcessInitialCommands);
        applyTransforms(drawableHitObject, commands);
    }

    public void UpdateStartTimeStateTransforms(DrawableHitObject drawableHitObject)
    {
        var commands = commandProvider.GetStartTimeStateCommands(drawableHitObject.HitObject);
        double startTimeOffset = -commandProvider.GetStartTimeOffset(drawableHitObject.HitObject);

        commands = postProcessCommand(drawableHitObject.HitObject, commands, x => x.PostProcessStartTimeStateCommands);
        applyTransforms(drawableHitObject, commands, startTimeOffset);
    }

    public void UpdateHitStateTransforms(DrawableHitObject drawableHitObject, ArmedState state)
    {
        if (state == ArmedState.Idle)
            return;

        var commands = commandProvider.GetHitStateCommands(drawableHitObject.HitObject, state);

        commands = postProcessCommand(drawableHitObject.HitObject, commands, x => x.PostProcessHitStateCommands);
        applyTransforms(drawableHitObject, commands);
    }

    private IEnumerable<IStageCommand> postProcessCommand(
        HitObject hitObject,
        IEnumerable<IStageCommand> commands,
        Func<IApplicableToStageHitObjectCommand, Func<HitObject, IEnumerable<IStageCommand>, IEnumerable<IStageCommand>>> postProcess)
    {
        return stageMods.Aggregate(commands, (current, mod) => postProcess(mod).Invoke(hitObject, current));
    }

    private static void applyTransforms<TDrawable>(TDrawable drawable, IEnumerable<IStageCommand> commands, double offset = 0)
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
