// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class DrawableNoteJudgement : DrawableJudgement
    {
        public DrawableNoteJudgement(JudgementResult result, DrawableHitObject judgedObject)
            : base(result, judgedObject)
        {
        }

        protected override void ApplyMissAnimations()
        {
            if (JudgementBody.Drawable is not DefaultKaraokeJudgementPiece)
            {
                // this is temporary logic until mania's skin transformer returns IAnimatableJudgements
                JudgementBody.ScaleTo(1.6f);
                JudgementBody.ScaleTo(1, 100, Easing.In);

                JudgementBody.MoveTo(Vector2.Zero);
                JudgementBody.MoveToOffset(new Vector2(0, 100), 800, Easing.InQuint);

                JudgementBody.RotateTo(0);
                JudgementBody.RotateTo(40, 800, Easing.InQuint);
                JudgementBody.FadeOutFromOne(800);

                LifetimeEnd = JudgementBody.LatestTransformEndTime;
            }

            base.ApplyMissAnimations();
        }

        protected override void ApplyHitAnimations()
        {
            JudgementBody.ScaleTo(0.8f);
            JudgementBody.ScaleTo(1, 250, Easing.OutElastic);

            JudgementBody.Delay(50)
                         .ScaleTo(0.75f, 250)
                         .FadeOut(200);
        }

        protected override Drawable CreateDefaultJudgement(HitResult result) => new DefaultKaraokeJudgementPiece(result);

        private class DefaultKaraokeJudgementPiece : DefaultJudgementPiece
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
        }
    }
}
