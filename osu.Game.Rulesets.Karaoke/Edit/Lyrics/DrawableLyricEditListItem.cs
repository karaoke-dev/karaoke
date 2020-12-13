// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Infos;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class DrawableLyricEditListItem : OsuRearrangeableListItem<Lyric>
    {
        private const int continuous_spacing = 20;
        private const int info_part_spacing = 200;
        private const int min_height = 75;
        private const int max_height = 120;

        private Box background;
        private Box dragAlert;
        

        public DrawableLyricEditListItem(Lyric item)
            : base(item)
        {
        }

        protected override Drawable CreateContent()
        {
            // todo : need to refactor this part.
            var isContinuous = Model.LayoutIndex == -1;
            var continuousSpacing = isContinuous ? continuous_spacing : 0;

            return new Container
            {
                Masking = true,
                CornerRadius = 5,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Margin = new MarginPadding
                {
                    Left = continuousSpacing,
                    Top = DrawableLyricEditList.SPACING,
                },
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.3f
                    },
                    dragAlert = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0
                    },
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.Absolute, info_part_spacing - continuousSpacing),
                            new Dimension(GridSizeMode.Distributed)
                        },
                        RowDimensions = new[] { new Dimension(GridSizeMode.AutoSize, minSize: min_height, maxSize: max_height) },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new InfoControl(Model)
                                {
                                    // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
                                    RelativeSizeAxes = Axes.X,
                                    Height = min_height,
                                },
                                new LyricControl(Model)
                                {
                                    Margin = new MarginPadding { Left = 10 },
                                    RelativeSizeAxes = Axes.X,
                                }
                            }
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray7;
            dragAlert.Colour = colours.YellowDarker;
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            if (!base.OnDragStart(e))
                return false;

            dragAlert.Show();
            return true;
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            dragAlert.Hide();
            base.OnDragEnd(e);
        }
    }
}
