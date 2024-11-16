// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Stages.Commands;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages;

/// <summary>
/// Provide commands for <see cref="DrawableKaraokeHitObject"/> by <see cref="HitObject"/>,
/// </summary>
public interface IHitObjectCommandProvider
{
    /// <summary>
    /// Get the preempt time for the <see cref="DrawableKaraokeHitObject"/>.
    /// </summary>
    /// <param name="hitObject"></param>
    /// <returns></returns>
    double GetPreemptTime(HitObject hitObject);

    /// <summary>
    /// Get the offset time between <see cref="HitObject.StartTime"/> and stage start time.
    /// </summary>
    /// <param name="hitObject"></param>
    /// <returns></returns>
    double GetStartTimeOffset(HitObject hitObject);

    /// <summary>
    /// Get the offset time between <see cref="HitObjectExtensions.GetEndTime"/> and stage start time.
    /// </summary>
    /// <param name="hitObject"></param>
    /// <returns></returns>
    double GetEndTimeOffset(HitObject hitObject);

    /// <summary>
    /// Get the list of <see cref="IStageCommand"/> that apply (generally fade-in) transforms leading into the <see cref="DrawableKaraokeHitObject"/> start time.
    /// </summary>
    /// <param name="hitObject"></param>
    IEnumerable<IStageCommand> GetInitialCommands(HitObject hitObject);

    /// <summary>
    /// Get the list of <see cref="IStageCommand"/> that apply passive transforms at the <see cref="DrawableKaraokeHitObject"/>'s StartTime.
    /// </summary>
    /// <param name="hitObject"></param>
    IEnumerable<IStageCommand> GetStartTimeStateCommands(HitObject hitObject);

    /// <summary>
    /// Get the list of <see cref="IStageCommand"/> that transforms based on the current <see cref="ArmedState"/> for <see cref="DrawableKaraokeHitObject"/>
    /// This call is offset by (HitObject.EndTime + Result.Offset), equivalent to when the user hit the object.
    /// </summary>
    /// <param name="hitObject"></param>
    /// <param name="state"></param>
    IEnumerable<IStageCommand> GetHitStateCommands(HitObject hitObject, ArmedState state);
}
