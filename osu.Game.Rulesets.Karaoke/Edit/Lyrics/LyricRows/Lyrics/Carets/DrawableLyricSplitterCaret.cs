// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics.Carets
{
    public class DrawableLyricSplitterCaret : CompositeDrawable, IDrawableCaret
    {
        private readonly Container splitter;
        private readonly SpriteIcon splitIcon;

        public DrawableLyricSplitterCaret()
        {
            Width = 10;
            InternalChildren = new Drawable[]
            {
                splitIcon = new SpriteIcon
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                    Icon = FontAwesome.Solid.HandScissors,
                    X = 7,
                    Y = -5,
                    Size = new Vector2(10),
                },
                splitter = new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new Triangle
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.BottomCentre,
                            Scale = new Vector2(1, -1),
                            Size = new Vector2(10, 5),
                        },
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Y,
                            Width = 2,
                            EdgeSmoothness = new Vector2(1, 0)
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            splitter.Colour = colours.Red;
            splitIcon.Colour = colours.Yellow;
        }

        private bool preview;

        public bool Preview
        {
            get => preview;
            set
            {
                preview = value;
                splitter.Alpha = preview ? 0.5f : 1;
                splitIcon.Alpha = preview ? 1 : 0;
            }
        }
    }
}
