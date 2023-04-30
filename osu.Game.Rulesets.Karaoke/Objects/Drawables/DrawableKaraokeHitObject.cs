// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables;

public partial class DrawableKaraokeHitObject : DrawableHitObject<KaraokeHitObject>
{
    protected DrawableKaraokeHitObject(KaraokeHitObject hitObject)
        : base(hitObject)
    {
    }

    protected sealed override double InitialLifetimeOffset
    {
        get
        {
            if (HitObject is IHasEffectApplier hitObjectWithEffectApplier)
            {
                return hitObjectWithEffectApplier.EffectApplier.PreemptTime;
            }

            return base.InitialLifetimeOffset;
        }
    }

    protected override JudgementResult CreateResult(Judgement judgement) => new KaraokeJudgementResult(HitObject, judgement);

    protected override void UpdateInitialTransforms()
    {
        if (HitObject is IHasEffectApplier hitObjectWithEffectApplier)
        {
            hitObjectWithEffectApplier.EffectApplier.UpdateInitialTransforms(this);
        }
    }

    protected override void UpdateStartTimeStateTransforms()
    {
        if (HitObject is IHasEffectApplier hitObjectWithEffectApplier)
        {
            hitObjectWithEffectApplier.EffectApplier.UpdateStartTimeStateTransforms(this);
        }
    }

    protected override void UpdateHitStateTransforms(ArmedState state)
    {
        if (HitObject is IHasEffectApplier hitObjectWithEffectApplier)
        {
            hitObjectWithEffectApplier.EffectApplier.UpdateHitStateTransforms(this, state);
        }
    }
}
