// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Cursor
{
    public class TimeTagTooltip : BackgroundToolTip
    {
        private const int time_display_height = 25;

        private Box background;
        private readonly OsuSpriteText trackTimer;
        private readonly OsuSpriteText index;
        private readonly OsuSpriteText indexState;

        protected override float Padding => 5;

        public TimeTagTooltip()
        {
            Child = new GridContainer
            {
                AutoSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, time_display_height),
                    new Dimension(GridSizeMode.Absolute, BORDER),
                    new Dimension(GridSizeMode.AutoSize)
                },
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        trackTimer = new OsuSpriteText
                        {
                            Font = OsuFont.GetFont(size: 21, fixedWidth: true)
                        }
                    },
                    null,
                    new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(10),
                            Children = new[]
                            {
                                index = new OsuSpriteText
                                {
                                    Font = OsuFont.GetFont(size: 12)
                                },
                                indexState = new OsuSpriteText
                                {
                                    Font = OsuFont.GetFont(size: 12)
                                }
                            }
                        }
                    }
                }
            };
        }

        protected override Drawable SetBackground()
        {
            return background = new Box
            {
                RelativeSizeAxes = Axes.X,
                Height = time_display_height + BORDER
            };
        }

        public override bool SetContent(object content)
        {
            if (!(content is TimeTag timeTag))
                return false;

            trackTimer.Text = TimeTagUtils.FormattedString(timeTag);
            index.Text = $"Position: {timeTag.Index.Index}";
            indexState.Text = timeTag.Index.State == TextIndex.IndexState.Start ? "Start" : "End";

            return true;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray2;
            indexState.Colour = colours.Red;
        }
    }
}
