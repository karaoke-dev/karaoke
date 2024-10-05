// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables;

public partial class DrawableKaraokeHitObject : DrawableHitObject<KaraokeHitObject>
{
    private IStageHitObjectRunner? createCommandRunner()
    {
        if (HitObject is IHasCommandGenerator { CommandGenerator: not null } hasCommandGenerator)
            return new StageHitObjectRunner(hasCommandGenerator.CommandGenerator);

        return null;
    }


    protected DrawableKaraokeHitObject(KaraokeHitObject? hitObject)
        : base(hitObject!)
    {
    }

    protected sealed override double InitialLifetimeOffset
        => createCommandRunner()?.GetPreemptTime(this) ?? base.InitialLifetimeOffset;

    protected override JudgementResult CreateResult(Judgement judgement) => new KaraokeJudgementResult(HitObject, judgement);

    protected override void UpdateInitialTransforms()
        => createCommandRunner()?.UpdateInitialTransforms(this);

    protected override void UpdateStartTimeStateTransforms()
        => createCommandRunner()?.UpdateStartTimeStateTransforms(this);

    protected override void UpdateHitStateTransforms(ArmedState state)
        => createCommandRunner()?.UpdateHitStateTransforms(this, state);
}
