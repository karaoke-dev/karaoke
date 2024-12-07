// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public interface IStageHitObjectRunner
{
    event Action? OnCommandUpdated;

    /// <summary>
    /// Get the preempt time for the <see cref="DrawableKaraokeHitObject"/>.
    /// </summary>
    /// <param name="hitObject"></param>
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
    /// Apply (generally fade-in) transforms leading into the <see cref="KaraokeHitObject"/> start time.
    /// </summary>
    /// <param name="drawableHitObject"></param>
    void UpdateInitialTransforms(DrawableHitObject drawableHitObject);

    /// <summary>
    /// Apply passive transforms at the <see cref="KaraokeHitObject"/>'s StartTime.
    /// </summary>
    /// <param name="drawableHitObject"></param>
    void UpdateStartTimeStateTransforms(DrawableHitObject drawableHitObject);

    /// <summary>
    /// Apply transforms based on the current <see cref="ArmedState"/>.
    /// This call is offset by (HitObject.EndTime + Result.Offset), equivalent to when the user hit the object.
    /// </summary>
    /// <param name="drawableHitObject"></param>
    /// <param name="state"></param>
    void UpdateHitStateTransforms(DrawableHitObject drawableHitObject, ArmedState state);
}
