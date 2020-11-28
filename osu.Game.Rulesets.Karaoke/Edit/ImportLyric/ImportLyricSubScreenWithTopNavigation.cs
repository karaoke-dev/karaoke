// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreenWithTopNavigation : ImportLyricSubScreen
    {
        protected TopNavigation Navigation { get; private set; }

        public ImportLyricSubScreenWithTopNavigation()
        {
            Padding = new MarginPadding(10);
            InternalChild = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 40),
                    new Dimension(GridSizeMode.Absolute, 10),
                    new Dimension(GridSizeMode.Distributed)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        Navigation = CreateNavigation(),
                    },
                    new Drawable[] { },
                    new Drawable[]
                    {
                        CreateContent(),
                    }
                }
            };
        }

        protected abstract TopNavigation CreateNavigation();

        protected abstract Drawable CreateContent();

        public abstract class TopNavigation : Container
        {
            [Resolved]
            protected OsuColour Colours { get; private set; }

            protected ImportLyricSubScreen Screen { get; private set; }

            private readonly CornerBackground background;
            private readonly OsuSpriteText text;
            private readonly IconButton button;

            public TopNavigation(ImportLyricSubScreen screen)
            {
                Screen = screen;

                RelativeSizeAxes = Axes.Both;
                InternalChildren = new Drawable[]
                {
                    background = new CornerBackground
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    text = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Margin = new MarginPadding{ Left = 15 }
                    },
                    button = new IconButton
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Margin = new MarginPadding{ Right = 5 },
                        Action = () =>
                        {
                            if (AbleToNextStep(State))
                            {
                                CompleteClicked();
                            }
                        }
                    }
                };
            }

            protected string NavigationText { get => text.Text; set => text.Text = value; }

            protected string TooltipText { get => button.TooltipText; set => button.TooltipText = value; }

            private NavigationState state;
            public NavigationState State {
                get => state;
                set
                {
                    state = value;
                    UpdateState(State);
                }
            }

            protected virtual void UpdateState(NavigationState value)
            {
                switch (value)
                {
                    case NavigationState.Initial:
                        background.Colour = Colours.Gray2;
                        text.Colour = Colours.GrayF;
                        button.Colour = Colours.Gray6;
                        button.Icon = FontAwesome.Regular.QuestionCircle;
                        break;
                    case NavigationState.Working:
                        background.Colour = Colours.Gray2;
                        text.Colour = Colours.GrayF;
                        button.Colour = Colours.Gray6;
                        button.Icon = FontAwesome.Solid.InfoCircle;
                        break;
                    case NavigationState.Done:
                        background.Colour = Colours.Gray6;
                        text.Colour = Colours.GrayF;
                        button.Colour = Colours.Yellow;
                        button.Icon = FontAwesome.Regular.ArrowAltCircleRight;
                        break;
                    case NavigationState.Error:
                        background.Colour = Colours.Gray2;
                        text.Colour = Colours.GrayF;
                        button.Colour = Colours.Yellow;
                        button.Icon = FontAwesome.Solid.ExclamationTriangle;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Should not goes to here");
                }
            }

            protected virtual bool AbleToNextStep(NavigationState value) => value == NavigationState.Done;

            protected virtual void CompleteClicked() => Screen.Complete();
        }

        public enum NavigationState
        {
            Initial,

            Working,

            Done,

            Error
        }
    }
}
