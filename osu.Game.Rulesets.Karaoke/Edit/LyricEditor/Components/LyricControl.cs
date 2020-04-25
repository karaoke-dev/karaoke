// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components.Badges;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components
{
    public class LyricControl : Container
    {
        private readonly Box background;

        public LyricControl(LyricLine lyric)
        {
            Masking = true;
            CornerRadius = 5;
            AutoSizeAxes = Axes.Y;
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.3f
                },
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    Margin = new MarginPadding(5),
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer<Badge>
                        {
                            Direction = FillDirection.Horizontal,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(5),
                            Children = new Badge[]
                            {
                                new TimeInfoBadge(lyric),
                                new StyleInfoBadge(lyric),
                                new LayoutInfoBadge(lyric),
                            }
                        },
                        new DrawableEditorLyricLine(lyric)
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray7;
        }

        public class DrawableEditorLyricLine : DrawableLyricLine
        {
            public DrawableEditorLyricLine(LyricLine lyric)
                : base(lyric)
            {
                DisplayRuby = true;
                DisplayRomaji = true;
            }

            protected override void ApplyLayout(KaraokeLayout layout)
            {
                base.ApplyLayout(new KaraokeLayout
                {
                    Name = "Edit layout",
                    Alignment = Anchor.TopLeft
                });
                Padding = new MarginPadding(0);
            }

            protected override void UpdateStateTransforms(ArmedState state)
            {
                // Do not fade-in / fade-out while changing armed state.
            }

            public override double LifetimeStart
            {
                get => double.MinValue;
                set => base.LifetimeStart = double.MinValue;
            }

            public override double LifetimeEnd
            {
                get => double.MaxValue;
                set => base.LifetimeEnd = double.MaxValue;
            }
        }
    }
}
