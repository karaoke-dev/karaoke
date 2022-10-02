// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor
{
    public abstract class BaseBottomEditor : CompositeDrawable
    {
        private const int info_part_spacing = 210;

        public abstract float ContentHeight { get; }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            Height = ContentHeight;
            RelativeSizeAxes = Axes.X;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background5(state.Mode)
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Absolute, info_part_spacing),
                        new Dimension()
                    },
                    RowDimensions = new[] { new Dimension(GridSizeMode.Relative) },
                    Content = new[]
                    {
                        new[]
                        {
                            CreateInfo(),
                            new Container
                            {
                                Masking = true,
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Child = CreateContent(),
                            }
                        }
                    }
                }
            };
        }

        protected abstract Drawable CreateInfo();

        protected abstract Drawable CreateContent();

        protected override bool OnDragStart(DragStartEvent e)
        {
            // prevent scroll container drag event.
            return true;
        }
    }
}
