// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Karaoke.UI;

public partial class DrawableNoteJudgement : DrawableJudgement
{
    public DrawableNoteJudgement(JudgementResult result, DrawableHitObject judgedObject)
        : base(result, judgedObject)
    {
    }

    public DrawableNoteJudgement()
    {
    }

    protected override Drawable CreateDefaultJudgement(HitResult result) => new DefaultKaraokeJudgementPiece(result);

    private partial class DefaultKaraokeJudgementPiece : DefaultJudgementPiece
    {
        public DefaultKaraokeJudgementPiece(HitResult result)
            : base(result)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            JudgementText.Font = JudgementText.Font.With(size: 25);
        }

        public override void PlayAnimation()
        {
            switch (Result)
            {
                case HitResult.None:
                case HitResult.Miss:
                    base.PlayAnimation();
                    break;

                default:
                    this.ScaleTo(0.8f);
                    this.ScaleTo(1, 250, Easing.OutElastic);

                    this.Delay(50)
                        .ScaleTo(0.75f, 250)
                        .FadeOut(200);

                    // karaoke uses a custom fade length, so the base call is intentionally omitted.
                    break;
            }
        }
    }
}
