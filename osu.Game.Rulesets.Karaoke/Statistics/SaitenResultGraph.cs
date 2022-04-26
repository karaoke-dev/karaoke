// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class SaitenResultGraph : CompositeDrawable
    {
        private readonly Box background;
        private readonly SaitenResultLyricPreview lyricGraph;
        private readonly NoteGraph noteGraph;

        public SaitenResultGraph(ScoreInfo score, IBeatmap beatmap)
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            Name = "Background",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                        },
                        lyricGraph = new SaitenResultLyricPreview(beatmap)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Spacing = new Vector2(5),
                        },
                        noteGraph = new NoteGraph(score)
                    },
                },
            };

            lyricGraph.SelectedLyric.BindValueChanged(e =>
            {
                // todo : move noteGraph to target time.
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.ContextMenuGray;
        }

        // todo: refactor needed.
        public class LyricPreview : CompositeDrawable
        {
            public Bindable<Lyric> SelectedLyric { get; } = new();

            private readonly FillFlowContainer<ClickableLyric> lyricTable;

            public LyricPreview(IEnumerable<Lyric> lyrics)
            {
                InternalChild = new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = lyricTable = new FillFlowContainer<ClickableLyric>
                    {
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Children = lyrics.Select(x => CreateLyricContainer(x).With(c =>
                        {
                            c.Selected = false;
                            c.Action = () => triggerLyric(x);
                        })).ToList()
                    }
                };

                SelectedLyric.BindValueChanged(value =>
                {
                    var oldValue = value.OldValue;
                    if (oldValue != null)
                        lyricTable.Where(x => x.Lyric == oldValue).ForEach(x => { x.Selected = false; });

                    var newValue = value.NewValue;
                    if (newValue != null)
                        lyricTable.Where(x => x.Lyric == newValue).ForEach(x => { x.Selected = true; });
                });
            }

            private void triggerLyric(Lyric lyric)
            {
                if (SelectedLyric.Value == lyric)
                    SelectedLyric.TriggerChange();
                else
                    SelectedLyric.Value = lyric;
            }

            public Vector2 Spacing
            {
                get => lyricTable.Spacing;
                set => lyricTable.Spacing = value;
            }

            protected virtual ClickableLyric CreateLyricContainer(Lyric lyric) => new(lyric);

            public class ClickableLyric : ClickableContainer
            {
                private const float fade_duration = 100;

                private Color4 hoverTextColour;
                private Color4 idolTextColour;

                private readonly Box background;
                private readonly Drawable icon;
                private readonly DrawableLyricSpriteText drawableLyric;

                public Lyric Lyric;

                public ClickableLyric(Lyric lyric)
                {
                    Lyric = lyric;

                    AutoSizeAxes = Axes.Y;
                    RelativeSizeAxes = Axes.X;
                    Masking = true;
                    CornerRadius = 5;
                    Children = new[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        icon = CreateIcon(),
                        drawableLyric = CreateLyric(lyric),
                    };
                }

                protected virtual DrawableLyricSpriteText CreateLyric(Lyric lyric) => new(lyric)
                {
                    Font = new FontUsage(size: 25),
                    RubyFont = new FontUsage(size: 10),
                    RomajiFont = new FontUsage(size: 10),
                    Margin = new MarginPadding { Left = 25 }
                };

                protected virtual Drawable CreateIcon() => new SpriteIcon
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(15),
                    Icon = FontAwesome.Solid.Play,
                    Margin = new MarginPadding { Left = 5 }
                };

                private bool selected;

                public bool Selected
                {
                    get => selected;
                    set
                    {
                        if (value == selected) return;

                        selected = value;

                        background.FadeTo(Selected ? 1 : 0, fade_duration);
                        icon.FadeTo(Selected ? 1 : 0, fade_duration);
                        drawableLyric.FadeColour(Selected ? hoverTextColour : idolTextColour, fade_duration);
                    }
                }

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    hoverTextColour = colours.Yellow;
                    idolTextColour = colours.Gray9;

                    drawableLyric.Colour = idolTextColour;
                    background.Colour = colours.Blue;
                    background.Alpha = 0;
                    icon.Colour = hoverTextColour;
                    icon.Alpha = 0;
                }
            }
        }

        private class SaitenResultLyricPreview : LyricPreview
        {
            public SaitenResultLyricPreview(IBeatmap beatmap)
                : base(beatmap.HitObjects.OfType<Lyric>())
            {
            }

            protected override ClickableLyric CreateLyricContainer(Lyric lyric)
                => new SaitenResultClickableLyric(lyric);

            private class SaitenResultClickableLyric : ClickableLyric
            {
                public SaitenResultClickableLyric(Lyric lyric)
                    : base(lyric)
                {
                }

                protected override DrawableLyricSpriteText CreateLyric(Lyric lyric)
                    => new(lyric)
                    {
                        Font = new FontUsage(size: 15),
                        RubyFont = new FontUsage(size: 7),
                        RomajiFont = new FontUsage(size: 7),
                        Margin = new MarginPadding { Left = 5 }
                    };

                protected override Drawable CreateIcon()
                    => new Container();
            }
        }

        private class NoteGraph : CompositeDrawable
        {
            public NoteGraph(ScoreInfo score)
            {
                var noteEvents = score.HitEvents?.Where(x => x.HitObject is Note { Display: true }).ToList() ?? new List<HitEvent>();

                foreach (var noteEvent in noteEvents)
                {
                    // TODO : add note into here
                }

                // todo : add list of note colors to present state.
            }

            internal class DrawableNote : Box
            {
                internal DrawableNote(HitResult result)
                {
                    // TODO : assign color with different hit result.
                }
            }
        }
    }
}
