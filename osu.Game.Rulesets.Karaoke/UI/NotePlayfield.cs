// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class NotePlayfield : ScrollingPlayfield, IKeyBindingHandler<KaraokeSoundAction>
    {
        [Resolved]
        private IPositionCalculator calculator { get; set; }

        public const float COLUMN_SPACING = 1;

        private readonly FillFlowContainer<ColumnBackground> columnFlow;

        private readonly Container judgementArea;
        private readonly JudgementContainer<DrawableNoteJudgement> judgements;

        private readonly Container hitObjectArea;
        private readonly Container hitObjectsContainer;
        private readonly CenterLine centerLine;
        private readonly RealTimeSaitenVisualization realTimeSaitenVisualization;
        private readonly SaitenVisualization replaySaitenVisualization;
        private readonly SaitenMarker saitenMarker;
        private readonly JudgelineMarker judgementLine;

        public int Columns { get; private set; }

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
                                new Box
                                {
                                    Name = "Background",
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.Black,
                                    Alpha = 0.5f
                                },
                                columnFlow = new FillFlowContainer<ColumnBackground>
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
                                    Children = new Drawable[]
                                    {
                                        judgements = new JudgementContainer<DrawableNoteJudgement>
                                        {
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            AutoSizeAxes = Axes.Both,
                                            BypassAutoSizeAxes = Axes.Both
                                        },
                                        judgementLine = new JudgelineMarker(),
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
                                        replaySaitenVisualization = new SaitenVisualization(this)
                                        {
                                            Name = "Saiten Visualization",
                                            RelativeSizeAxes = Axes.Both,
                                            PathRadius = 1.5f,
                                            Alpha = 0.6f
                                        },
                                        realTimeSaitenVisualization = new RealTimeSaitenVisualization(this)
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
                        }
                    }
                }
            };

            for (int i = 0; i < columns; i++)
            {
                var column = new ColumnBackground(i)
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
                var juggementPadding = 10;

                judgementArea.Size = new Vector2(judgementAreaPercentage, 1);
                judgementArea.X = left ? 0 : (1 - judgementAreaPercentage);

                judgementLine.Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft;
                saitenMarker.Anchor = saitenMarker.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
                saitenMarker.Scale = left ? new Vector2(1, 1) : new Vector2(-1, 1);

                judgements.Anchor = judgements.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
                judgements.X = left ? -juggementPadding : juggementPadding;

                hitObjectArea.Size = new Vector2(1 - judgementAreaPercentage, 1);
                hitObjectArea.X = left ? judgementAreaPercentage : 0;

                realTimeSaitenVisualization.Anchor = left ? Anchor.CentreLeft : Anchor.CentreRight;
                realTimeSaitenVisualization.Origin = left ? Anchor.CentreRight : Anchor.CentreLeft;
            }, true);
        }

        public void AddColumn(ColumnBackground c)
        {
            columnFlow.Add(c);
        }

        public override void Add(DrawableHitObject h)
        {
            var note = (Note)h.HitObject;

            note.ToneBindable.BindValueChanged(tone =>
            {
                h.Y = calculator.YPositionAt(tone.NewValue);
            }, true);

            h.OnNewResult += OnNewResult;

            base.Add(h);
        }

        public override bool Remove(DrawableHitObject h)
        {
            if (!base.Remove(h))
                return false;

            h.OnNewResult -= OnNewResult;
            return true;
        }

        public void Add(BarLine barline) => base.Add(new DrawableBarLine(barline));

        public void AddReplay(KaraokeReplayFrame frame)
        {
            replaySaitenVisualization.Add(frame);
        }

        internal void OnNewResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            if (!judgedObject.DisplayResult || !DisplayJudgements.Value)
                return;

            if (judgedObject is DrawableNote note)
            {
                judgements.Clear();
                judgements.Add(new DrawableNoteJudgement(result, judgedObject)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = calculator.YPositionAt(note.HitObject.Tone + 2)
                });
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            columnFlow.Children.ForEach(x => x.Colour = x.IsSpecial ? colours.Gray9 : colours.Gray0);
            replaySaitenVisualization.LineColour = Color4.White;
            realTimeSaitenVisualization.LineColour = colours.Yellow;
        }

        public bool OnPressed(KaraokeSoundAction action)
        {
            // TODO : appear marker and move position with delay time
            saitenMarker.Y = calculator.YPositionAt(action);
            saitenMarker.Alpha = 1;

            // Mark as singing
            realTimeSaitenVisualization.AddAction(action);

            return true;
        }

        public bool OnReleased(KaraokeSoundAction action)
        {
            // TODO : disappear marker
            saitenMarker.Alpha = 0;

            // Stop singing
            realTimeSaitenVisualization.Release();

            return true;
        }
    }
}
