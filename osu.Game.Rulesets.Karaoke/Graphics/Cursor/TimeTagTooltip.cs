// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public class TimeTagTooltip : BackgroundToolTip
    {
        private const int time_display_height = 35;

        private Box background;
        private readonly OsuSpriteText trackTimer;
        private readonly OsuSpriteText index;
        private readonly OsuSpriteText indexState;

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
                            Font = OsuFont.GetFont(size: 25, fixedWidth: true)
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
                                index = new OsuSpriteText(),
                                indexState = new OsuSpriteText()
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
                Height = time_display_height + BORDER * 2
            };
        }

        public override bool SetContent(object content)
        {
            if (!(content is TimeTag timeTag))
                return false;

            trackTimer.Text = timeTag.Time?.ToEditorFormattedString() ?? "--:--:---";
            index.Text = $"At index {timeTag.Index.Index}";
            indexState.Text = timeTag.Index.State == TimeTagIndex.IndexState.Start ? "Start" : "End";

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
