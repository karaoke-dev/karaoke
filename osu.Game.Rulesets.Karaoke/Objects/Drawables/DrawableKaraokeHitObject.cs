// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Stages.Drawables;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables;

public partial class DrawableKaraokeHitObject : DrawableHitObject<KaraokeHitObject>
{
    [Resolved]
    private IStageHitObjectRunner? stageRunner { get; set; }

    protected DrawableKaraokeHitObject(KaraokeHitObject? hitObject)
        : base(hitObject!)
    {
    }

    protected sealed override double InitialLifetimeOffset
        => stageRunner?.GetStartTimeOffset(HitObject) ?? base.InitialLifetimeOffset;

    protected override JudgementResult CreateResult(Judgement judgement) => new KaraokeJudgementResult(HitObject, judgement);

    protected override void UpdateInitialTransforms()
        => stageRunner?.UpdateInitialTransforms(this);

    protected override void UpdateStartTimeStateTransforms()
        => stageRunner?.UpdateStartTimeStateTransforms(this);

    protected override void UpdateHitStateTransforms(ArmedState state)
        => stageRunner?.UpdateHitStateTransforms(this, state);
}
