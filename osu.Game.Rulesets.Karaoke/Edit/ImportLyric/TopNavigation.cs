// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class TopNavigation<T> : TopNavigation where T : LyricImporterStepScreenWithTopNavigation
    {
        protected new T Screen => base.Screen as T;

        protected TopNavigation(T screen)
            : base(screen)
        {
        }
    }

    public abstract class TopNavigation : CompositeDrawable
    {
        [Resolved]
        protected OsuColour Colours { get; private set; }

        protected LyricImporterStepScreen Screen { get; }

        private readonly CornerBackground background;
        private readonly NavigationTextContainer text;
        private readonly IconButton button;

        protected TopNavigation(LyricImporterStepScreen screen)
        {
            Screen = screen;

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                background = new CornerBackground
                {
                    RelativeSizeAxes = Axes.Both,
                },
                text = CreateTextContainer().With(t =>
                {
                    t.Anchor = Anchor.CentreLeft;
                    t.Origin = Anchor.CentreLeft;
                    t.RelativeSizeAxes = Axes.X;
                    t.AutoSizeAxes = Axes.Y;
                    t.Margin = new MarginPadding { Left = 15 };
                }),
                button = new IconButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Margin = new MarginPadding { Right = 5 },
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

        protected abstract NavigationTextContainer CreateTextContainer();

        protected string NavigationText
        {
            set => text.Text = value;
        }

        protected LocalisableString TooltipText
        {
            get => button.TooltipText;
            set => button.TooltipText = value;
        }

        private NavigationState state;

        public NavigationState State
        {
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
                    throw new ArgumentOutOfRangeException(nameof(value));
            }

            // Force change style if this step is able to go to next step.
            if (AbleToNextStep(value))
            {
                button.Icon = FontAwesome.Regular.ArrowAltCircleRight;
            }
        }

        protected virtual bool AbleToNextStep(NavigationState value)
            => value == NavigationState.Done;

        protected virtual void CompleteClicked() => Screen.Complete();

        public class NavigationTextContainer : CustomizableTextContainer
        {
            protected void AddLinkFactory(string name, string text, Action action)
            {
                AddIconFactory(name, () => new ClickableSpriteText
                {
                    Font = new FontUsage(size: 20),
                    Text = text,
                    Action = action
                });
            }

            internal class ClickableSpriteText : OsuSpriteText
            {
                public Action Action { get; set; }

                protected override bool OnClick(ClickEvent e)
                {
                    Action?.Invoke();
                    return base.OnClick(e);
                }

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    Colour = colours.Yellow;
                }
            }
        }
    }

    public enum NavigationState
    {
        Initial,

        Working,

        Done,

        Error
    }
}
