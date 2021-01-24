// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.Containers
{
    public abstract class TitleFocusedOverlayContainer : PopupFocusedOverlayContainer
    {
        private readonly Box background;
        private readonly IconButton closeButton;
        private readonly Box contentBackground;
        private readonly Container content;

        protected override Container<Drawable> Content => content;

        protected virtual string Title => "Title";

        protected TitleFocusedOverlayContainer()
        {
            Masking = true;
            CornerRadius = 10;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Children = new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = Title,
                                            Font = OsuFont.GetFont(size: 30),
                                            Padding = new MarginPadding { Vertical = 10 },
                                        },
                                        closeButton = new IconButton
                                        {
                                            Anchor = Anchor.CentreRight,
                                            Origin = Anchor.CentreRight,
                                            Icon = FontAwesome.Solid.Times,
                                            Scale = new Vector2(0.8f),
                                            X = -10,
                                            Action = () => State.Value = Visibility.Hidden
                                        }
                                    }
                                }
                            },
                            new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        contentBackground = new Box
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                        },
                                        content = new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                        },
                                    }
                                }
                            },
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.GreySeafoamDark;
            closeButton.Colour = colours.GreySeafoamDarker;
            contentBackground.Colour = colours.GreySeafoamDarker;
        }
    }
}
