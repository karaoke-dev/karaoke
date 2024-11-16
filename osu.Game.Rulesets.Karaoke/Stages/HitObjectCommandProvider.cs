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

public abstract class HitObjectCommandProvider<TStageInfo, THitObject> : IHitObjectCommandProvider
    where THitObject : KaraokeHitObject
    where TStageInfo : StageInfo
{
    protected readonly TStageInfo StageInfo;

    protected HitObjectCommandProvider(TStageInfo stageInfo)
    {
        StageInfo = stageInfo;
    }

    public double GetPreemptTime(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GeneratePreemptTime(tHitObject);
    }

    public double GetStartTimeOffset(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        (double? startTime, _) = GetStartAndEndTime(tHitObject);

        if (startTime == null)
            return 0;


        return hitObject.StartTime - startTime.Value;
    }

    public double GetEndTimeOffset(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        (_, double? endTime) = GetStartAndEndTime(tHitObject);

        if (endTime == null)
            return 0;

        return endTime.Value - hitObject.GetEndTime();
    }

    public IEnumerable<IStageCommand> GetInitialCommands(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GetInitialCommands(tHitObject);
    }

    public IEnumerable<IStageCommand> GetStartTimeStateCommands(HitObject hitObject)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GetStartTimeStateCommands(tHitObject);
    }

    public IEnumerable<IStageCommand> GetHitStateCommands(HitObject hitObject, ArmedState state)
    {
        if (hitObject is not THitObject tHitObject)
            throw new InvalidCastException();

        return GetHitStateCommands(tHitObject, state);
    }

    protected abstract double GeneratePreemptTime(THitObject hitObject);

    protected abstract IEnumerable<IStageCommand> GetInitialCommands(THitObject hitObject);

    protected abstract Tuple<double?, double?> GetStartAndEndTime(THitObject lyric);

    protected abstract IEnumerable<IStageCommand> GetStartTimeStateCommands(THitObject hitObject);

    protected abstract IEnumerable<IStageCommand> GetHitStateCommands(THitObject hitObject, ArmedState state);
}
