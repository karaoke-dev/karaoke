// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;
using System;

namespace osu.Game.Rulesets.Karaoke.UI.Scrolling
{
    public abstract class ScrollingNotePlayfield : ScrollingPlayfield
    {
        [Resolved]
        protected IPositionCalculator Calculator { get; private set; }

        public const float COLUMN_SPACING = 1;

        private readonly FillFlowContainer<DefaultColumnBackground> columnFlow;

        protected readonly Container BackgroundLayer;
        protected readonly Container HitObjectLayer;
        protected readonly Container HitObjectArea;

        public int Columns { get; }

        public ScrollingNotePlayfield(int columns)
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
                    Masking = true,
                    CornerRadius = 5,
                    Children = new Drawable[]
                    {
                        BackgroundLayer = new Container
                        {
                            Name = "Background mask",
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Masking = true,
                            CornerRadius = 5,
                            Children = new Drawable[]
                            {
                                // background
                                columnFlow = new FillFlowContainer<DefaultColumnBackground>
                                {
                                    Name = "Columns",
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Top = COLUMN_SPACING, Bottom = COLUMN_SPACING },
                                    Spacing = new Vector2(0, COLUMN_SPACING)
                                },
                                // center line
                            }
                        },
                        HitObjectLayer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                // judgement
                                HitObjectArea = new Container
                                {
                                    Depth = 1,
                                    RelativeSizeAxes = Axes.Both,
                                    RelativePositionAxes = Axes.X,
                                    Children = new Drawable[]
                                    {
                                        new Container
                                        {
                                            Name = "Hit objects",
                                            RelativeSizeAxes = Axes.Both,
                                            Child = HitObjectContainer
                                        },
                                        // scoring visualization
                                    }
                                }
                            },
                        },
                    }
                },
                // other things like microphone status
            };

            for (int i = 0; i < columns; i++)
            {
                var column = new DefaultColumnBackground(i)
                {
                    IsSpecial = i % 2 == 0
                };

                columnFlow.Add(column);
            }

            Direction.BindValueChanged(dir =>
            {
                switch (dir.NewValue)
                {
                    case ScrollingDirection.Left:
                        OnDirectionChanged(KaraokeScrollingDirection.Left);
                        break;
                    case ScrollingDirection.Right:
                        OnDirectionChanged(KaraokeScrollingDirection.Right);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dir.NewValue));
                }
            });
        }

        protected virtual void OnDirectionChanged(KaraokeScrollingDirection direction)
        {
            bool left = direction == KaraokeScrollingDirection.Left;
            //TODO : will apply in skin
            var judgementAreaPercentage = 0.4f;
            HitObjectArea.Size = new Vector2(1 - judgementAreaPercentage, 1);
            HitObjectArea.X = left ? judgementAreaPercentage : 0;
        }

        protected override void OnNewDrawableHitObject(DrawableHitObject drawableHitObject)
        {
            if (drawableHitObject is DrawableNote drawableNote)
            {
                drawableNote.ToneBindable.BindValueChanged(tone =>
                {
                    drawableHitObject.Y = Calculator.YPositionAt(tone.NewValue);
                });
            }

            base.OnNewDrawableHitObject(drawableHitObject);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            columnFlow.Children.ForEach(x => x.Colour = x.IsSpecial ? colours.Gray9 : colours.Gray0);

            RegisterPool<Note, DrawableNote>(50);
            RegisterPool<BarLine, DrawableBarLine>(15);
        }
    }
}
