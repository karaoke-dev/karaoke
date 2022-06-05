// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
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
        private readonly BindableInt saitenPitch = new();

        private readonly CenterLine centerLine;

        private readonly Container judgementArea;
        private readonly JudgementContainer<DrawableNoteJudgement> judgements;
        private readonly Drawable judgementLine;
        private readonly SaitenMarker saitenMarker;

        private readonly RealTimeSaitenVisualization realTimeSaitenVisualization;
        private readonly ReplaySaitenVisualization replaySaitenVisualization;

        private readonly SaitenStatus saitenStatus;

        // Note playfield should be present even being hidden.
        public override bool IsPresent => true;

        [Resolved]
        private INotePositionInfo notePositionInfo { get; set; }

        public NotePlayfield(int columns)
            : base(columns)
        {
            if (InternalChildren.FirstOrDefault() is Container container)
            {
                // add padding to first children.
                container.Padding = new MarginPadding { Top = 30, Bottom = 30 };
            }

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
                // todo : generate this only if in auto-play mode.
                replaySaitenVisualization = new ReplaySaitenVisualization(null)
                {
                    Name = "Saiten Visualization",
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.6f
                },
                realTimeSaitenVisualization = new RealTimeSaitenVisualization
                {
                    Name = "Saiten Visualization",
                    RelativeSizeAxes = Axes.Both,
                },
            });

            AddInternal(saitenStatus = new SaitenStatus(SaitenStatusMode.NotInitialized));
        }

        protected override void OnDirectionChanged(KaraokeScrollingDirection direction, float judgementAreaPercentage)
        {
            base.OnDirectionChanged(direction, judgementAreaPercentage);

            bool left = direction == KaraokeScrollingDirection.Left;

            //TODO : will apply in skin
            int judgementPadding = 10;

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
                int newValue = value.NewValue;
                var targetTone = new Tone((newValue < 0 ? newValue - 1 : newValue) / 2, newValue % 2 != 0);
                float targetY = notePositionInfo.Calculator.YPositionAt(targetTone);
                float targetHeight = targetTone.Half ? 5 : DefaultColumnBackground.COLUMN_HEIGHT;
                float alpha = targetTone.Half ? 0.6f : 0.2f;

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

            if (judgedObject is not DrawableNote note)
                return;

            judgements.Clear();
            judgements.Add(new DrawableNoteJudgement(result, judgedObject)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Y = notePositionInfo.Calculator.YPositionAt(note.HitObject.Tone + 2)
            });

            // Add hit explosion
            if (!result.IsHit)
                return;

            var explosion = new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.HitExplosion), _ =>
                new DefaultHitExplosion(judgedObject.AccentColour.Value, judgedObject is DrawableNote))
            {
                Y = notePositionInfo.Calculator.YPositionAt(note.HitObject)
            };

            // todo : should be added into hitObjectArea.Explosions
            // see how mania ruleset do
            HitObjectArea.Add(explosion);

            explosion.Delay(200).Expire(true);
        }

        [BackgroundDependencyLoader(true)]
        private void load([CanBeNull] KaraokeSessionStatics session)
        {
            session?.BindWith(KaraokeRulesetSession.SaitenPitch, saitenPitch);

            session?.GetBindable<SaitenStatusMode>(KaraokeRulesetSession.SaitenStatus).BindValueChanged(e => { saitenStatus.SaitenStatusMode = e.NewValue; });
        }

        public bool OnPressed(KeyBindingPressEvent<KaraokeSaitenAction> e)
        {
            // TODO : appear marker and move position with delay time
            saitenMarker.Y = notePositionInfo.Calculator.YPositionAt(e.Action);
            saitenMarker.Alpha = 1;

            // Mark as singing
            realTimeSaitenVisualization.AddAction(e.Action);

            return true;
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeSaitenAction> e)
        {
            // TODO : disappear marker
            saitenMarker.Alpha = 0;

            // Stop singing
            realTimeSaitenVisualization.Release();
        }
    }
}
