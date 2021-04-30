// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class NotePlayfield : ScrollingNotePlayfield, IKeyBindingHandler<KaraokeSaitenAction>
    {
        private readonly BindableInt saitenPitch = new BindableInt();

        private readonly Container judgementArea;
        private readonly JudgementContainer<DrawableNoteJudgement> judgements;

        private readonly CenterLine centerLine;
        private readonly RealTimeSaitenVisualization realTimeSaitenVisualization;
        private readonly SaitenVisualization replaySaitenVisualization;
        private readonly SaitenMarker saitenMarker;
        private readonly Drawable judgementLine;

        private readonly SaitenStatus saitenStatus;

        public int Columns { get; }

        // Note playfield should be present even being hidden.
        public override bool IsPresent => true;

        public NotePlayfield(int columns)
            : base(columns)
        {
            Columns = columns;

            BackgroundLayer.AddRange(new Drawable[]
            {
                new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.StageBackground), _ => null)
                {
                    Depth = 2,
                    RelativeSizeAxes = Axes.Both
                },
                new Box
                {
                    Depth = 1,
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f
                },
                centerLine = new CenterLine
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            });

            HitObjectLayer.Add(judgementArea = new Container
            {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.X,
                Children = new[]
                {
                    judgements = new JudgementContainer<DrawableNoteJudgement>
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        BypassAutoSizeAxes = Axes.Both
                    },
                    judgementLine = new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.JudgementLine), _ => new DefaultJudgementLine())
                    {
                        RelativeSizeAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    saitenMarker = new SaitenMarker
                    {
                        Alpha = 0
                    }
                }
            });

            HitObjectArea.AddRange(new Drawable[]
            {
                replaySaitenVisualization = new SaitenVisualization
                {
                    Name = "Saiten Visualization",
                    RelativeSizeAxes = Axes.Both,
                    PathRadius = 1.5f,
                    Alpha = 0.6f
                },
                realTimeSaitenVisualization = new RealTimeSaitenVisualization
                {
                    Name = "Saiten Visualization",
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    PathRadius = 2.5f,
                    OrientatePosition = SaitenVisualization.SaitenOrientatePosition.End
                },
            });

            AddInternal(saitenStatus = new SaitenStatus(SaitenStatusMode.NotInitialized));
        }

        protected override void OnDirectionChanged(KaraokeScrollingDirection direction)
        {
            base.OnDirectionChanged(direction);

            bool left = direction == KaraokeScrollingDirection.Left;

            //TODO : will apply in skin
            var judgementAreaPercentage = 0.4f;
            var judgementPadding = 10;

            judgementArea.Size = new Vector2(judgementAreaPercentage, 1);
            judgementArea.X = left ? 0 : 1 - judgementAreaPercentage;

            judgementLine.Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft;
            saitenMarker.Anchor = saitenMarker.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
            saitenMarker.Scale = left ? new Vector2(1, 1) : new Vector2(-1, 1);

            judgements.Anchor = judgements.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
            judgements.X = left ? -judgementPadding : judgementPadding;

            realTimeSaitenVisualization.Anchor = left ? Anchor.CentreLeft : Anchor.CentreRight;
            realTimeSaitenVisualization.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            NewResult += OnNewResult;

            saitenPitch.BindValueChanged(value =>
            {
                var newValue = value.NewValue;
                var targetTone = new Tone((newValue < 0 ? newValue - 1 : newValue) / 2, newValue % 2 != 0);
                var targetY = Calculator.YPositionAt(targetTone);
                var targetHeight = targetTone.Half ? 5 : DefaultColumnBackground.COLUMN_HEIGHT;
                var alpha = targetTone.Half ? 0.6f : 0.2f;

                centerLine.MoveToY(targetY, 100);
                centerLine.ResizeHeightTo(targetHeight, 100);
                centerLine.Alpha = alpha;
            }, true);
        }

        public void ClearReplay()
        {
            replaySaitenVisualization.Clear();
        }

        public void AddReplay(KaraokeReplayFrame frame)
        {
            replaySaitenVisualization.Add(frame);
        }

        internal void OnNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            // Add judgement
            if (!judgedObject.DisplayResult || !DisplayJudgements.Value)
                return;

            if (!(judgedObject is DrawableNote note))
                return;

            judgements.Clear();
            judgements.Add(new DrawableNoteJudgement(result, judgedObject)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Y = Calculator.YPositionAt(note.HitObject.Tone + 2)
            });

            // Add hit explosion
            if (!result.IsHit)
                return;

            var explosion = new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.HitExplosion), _ =>
                new DefaultHitExplosion(judgedObject.AccentColour.Value, judgedObject is DrawableNote))
            {
                Y = Calculator.YPositionAt(note.HitObject.Tone)
            };

            // todo : should be added into hitObjectArea.Explosions
            // see how mania ruleset do
            HitObjectArea.Add(explosion);

            explosion.Delay(200).Expire(true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, KaraokeSessionStatics session)
        {
            replaySaitenVisualization.LineColour = Color4.White;
            realTimeSaitenVisualization.LineColour = colours.Yellow;

            session.BindWith(KaraokeRulesetSession.SaitenPitch, saitenPitch);

            session.GetBindable<SaitenStatusMode>(KaraokeRulesetSession.SaitenStatus).BindValueChanged(e => { saitenStatus.SaitenStatusMode = e.NewValue; });
        }

        public bool OnPressed(KaraokeSaitenAction action)
        {
            // TODO : appear marker and move position with delay time
            saitenMarker.Y = Calculator.YPositionAt(action);
            saitenMarker.Alpha = 1;

            // Mark as singing
            realTimeSaitenVisualization.AddAction(action);

            return true;
        }

        public void OnReleased(KaraokeSaitenAction action)
        {
            // TODO : disappear marker
            saitenMarker.Alpha = 0;

            // Stop singing
            realTimeSaitenVisualization.Release();
        }
    }
}
