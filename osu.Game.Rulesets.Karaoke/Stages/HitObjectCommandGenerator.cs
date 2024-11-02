// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Karaoke.Stages.Infos;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages;

public abstract class HitObjectCommandGenerator<TStageInfo, THitObject> : IHitObjectCommandGenerator
    where THitObject : KaraokeHitObject
    where TStageInfo : StageInfo
{
    protected readonly TStageInfo StageInfo;

    protected HitObjectCommandGenerator(TStageInfo stageInfo)
    {
        StageInfo = stageInfo;
    }

    public double GeneratePreemptTime(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GeneratePreemptTime(tHitObject);
    }

    public double GenerateStartTimeOffset(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        (double? startTime, _) = StageInfo.GetStartAndEndTime(tHitObject);

        if (startTime == null)
            return 0;


        return hitObject.StartTime - startTime.Value;
    }

    public double GenerateEndTimeOffset(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        (_, double? endTime) = StageInfo.GetStartAndEndTime(tHitObject);

        if (endTime == null)
            return 0;

        return endTime.Value - hitObject.GetEndTime();
    }

    public IEnumerable<IStageCommand> GenerateInitialCommands(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GenerateInitialCommands(tHitObject);
    }

    public IEnumerable<IStageCommand> GenerateStartTimeStateCommands(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GenerateStartTimeStateCommands(tHitObject);
    }

    public IEnumerable<IStageCommand> GenerateHitStateCommands(HitObject hitObject, ArmedState state)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GenerateHitStateCommands(tHitObject, state);
    }

    protected abstract double GeneratePreemptTime(THitObject hitObject);

    protected abstract IEnumerable<IStageCommand> GenerateInitialCommands(THitObject hitObject);

    protected abstract IEnumerable<IStageCommand> GenerateStartTimeStateCommands(THitObject hitObject);

    protected abstract IEnumerable<IStageCommand> GenerateHitStateCommands(THitObject hitObject, ArmedState state);
}
