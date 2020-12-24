// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
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
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class NotePlayfield : ScrollingPlayfield, IKeyBindingHandler<KaraokeSaitenAction>
    {
        [Resolved]
        private IPositionCalculator calculator { get; set; }

        public const float COLUMN_SPACING = 1;

        private readonly BindableInt saitenPitch = new BindableInt();

        private readonly FillFlowContainer<DefaultColumnBackground> columnFlow;

        private readonly Container judgementArea;
        private readonly JudgementContainer<DrawableNoteJudgement> judgements;

        private readonly Container hitObjectArea;
        private readonly Container hitObjectsContainer;
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
        {
            Columns = columns;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding { Top = 30, Bottom = 30 },
                    Masking = true,
                    CornerRadius = 5,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Name = "Background mask",
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Masking = true,
                            CornerRadius = 5,
                            Children = new Drawable[]
                            {
                                new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.StageBackground), _ => null)
                                {
                                    RelativeSizeAxes = Axes.Both
                                },
                                new Box
                                {
                                    Name = "Background",
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.Black,
                                    Alpha = 0.5f
                                },
                                columnFlow = new FillFlowContainer<DefaultColumnBackground>
                                {
                                    Name = "Columns",
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Top = COLUMN_SPACING, Bottom = COLUMN_SPACING },
                                    Spacing = new Vector2(0, COLUMN_SPACING)
                                },
                                centerLine = new CenterLine
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                }
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                judgementArea = new Container
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
                                },
                                hitObjectArea = new Container
                                {
                                    Depth = 1,
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.X,
                                    Children = new Drawable[]
                                    {
                                        hitObjectsContainer = new Container
                                        {
                                            Name = "Hit objects",
                                            RelativeSizeAxes = Axes.Both,
                                            Child = HitObjectContainer
                                        },
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
                                    }
                                }
                            },
                        },
                    }
                },
                saitenStatus = new SaitenStatus(SaitenStatusMode.NotInitialized)
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                }
            };

            for (int i = 0; i < columns; i++)
            {
                var column = new DefaultColumnBackground(i)
                {
                    IsSpecial = i % 2 == 0
                };

                AddColumn(column);
            }

            Direction.BindValueChanged(dir =>
            {
                bool left = dir.NewValue == ScrollingDirection.Left;

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

                hitObjectArea.Size = new Vector2(1 - judgementAreaPercentage, 1);
                hitObjectArea.X = left ? judgementAreaPercentage : 0;

                realTimeSaitenVisualization.Anchor = left ? Anchor.CentreLeft : Anchor.CentreRight;
                realTimeSaitenVisualization.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
            }, true);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            NewResult += OnNewResult;

            saitenPitch.BindValueChanged(value =>
            {
                var newValue = value.NewValue;
                var targetTone = new Tone((newValue < 0 ? newValue - 1 : newValue) / 2, newValue % 2 != 0);
                var targetY = calculator.YPositionAt(targetTone);
                var targetHeight = targetTone.Half ? 5 : DefaultColumnBackground.COLUMN_HEIGHT;
                var alpha = targetTone.Half ? 0.6f : 0.2f;

                centerLine.MoveToY(targetY, 100);
                centerLine.ResizeHeightTo(targetHeight, 100);
                centerLine.Alpha = alpha;
            }, true);
        }

        public void AddColumn(DefaultColumnBackground c)
        {
            columnFlow.Add(c);
        }

        public override void Add(DrawableHitObject h)
        {
            if (h is DrawableNote drawableNote)
            {
                drawableNote.ToneBindable.BindValueChanged(tone => { h.Y = calculator.YPositionAt(tone.NewValue); }, true);
            }

            base.Add(h);
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
                Y = calculator.YPositionAt(note.HitObject.Tone + 2)
            });

            // Add hit explosion
            if (!result.IsHit)
                return;

            var explosion = new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.HitExplosion), _ =>
                new DefaultHitExplosion(judgedObject.AccentColour.Value, judgedObject is DrawableNote))
            {
                Y = calculator.YPositionAt(note.HitObject.Tone)
            };

            // todo : should be added into hitObjectArea.Explosions
            // see how mania ruleset do
            hitObjectArea.Add(explosion);

            explosion.Delay(200).Expire(true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, KaraokeSessionStatics session)
        {
            columnFlow.Children.ForEach(x => x.Colour = x.IsSpecial ? colours.Gray9 : colours.Gray0);
            replaySaitenVisualization.LineColour = Color4.White;
            realTimeSaitenVisualization.LineColour = colours.Yellow;

            session.BindWith(KaraokeRulesetSession.SaitenPitch, saitenPitch);

            session.GetBindable<SaitenStatusMode>(KaraokeRulesetSession.SaitenStatus).BindValueChanged(e => { saitenStatus.SaitenStatusMode = e.NewValue; });

            RegisterPool<Note, DrawableNote>(50);
            RegisterPool<BarLine, DrawableBarLine>(15);
        }

        public bool OnPressed(KaraokeSaitenAction action)
        {
            // TODO : appear marker and move position with delay time
            saitenMarker.Y = calculator.YPositionAt(action);
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
