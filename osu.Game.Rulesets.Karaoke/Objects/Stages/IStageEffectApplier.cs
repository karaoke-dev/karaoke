// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Stages;

public interface IStageEffectApplier
{
    double PreemptTime { get; }

    void UpdateInitialTransforms(DrawableHitObject drawableHitObject);

    void UpdateStartTimeStateTransforms(DrawableHitObject drawableHitObject);

    void UpdateHitStateTransforms(DrawableHitObject drawableHitObject, ArmedState state);
}
