// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Stages.Drawables;

public interface IStageHitObjectRunner
{
    double GetPreemptTime(DrawableKaraokeHitObject hitObject);

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
