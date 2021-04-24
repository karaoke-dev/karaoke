// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays
{
    public abstract class EditOverlay : VisibilityContainer
    {
        private const int info_part_spacing = 210;
        private const float transition_duration = 600;

        protected abstract float ContentHeight { get; }

        private readonly Lyric lyric;

        protected EditOverlay(Lyric lyric)
        {
            this.lyric = lyric;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.8f,
                    Colour = Color4.Black
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Absolute, info_part_spacing),
                        new Dimension(GridSizeMode.Distributed)
                    },
                    RowDimensions = new[] { new Dimension(GridSizeMode.Relative) },
                    Content = new[]
                    {
                        new[]
                        {
                            CreateInfo(lyric),
                            CreateContent(lyric)
                        }
                    }
                }
            };
        }

        protected abstract Drawable CreateInfo(Lyric lyric);

        protected abstract Drawable CreateContent(Lyric lyric);

        protected override void PopIn()
        {
            this.ResizeTo(new Vector2(1, ContentHeight), transition_duration, Easing.OutQuint);
            this.FadeIn(transition_duration, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            this.ResizeTo(new Vector2(1, 0), transition_duration, Easing.OutQuint);
            this.FadeOut(transition_duration);
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            // prevent scroll container drag event.
            return true;
        }
    }
}
