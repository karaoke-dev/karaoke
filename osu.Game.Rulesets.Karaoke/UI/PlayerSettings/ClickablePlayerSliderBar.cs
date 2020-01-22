// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class ClickablePlayerSliderBar : SettingsSlider<int>
    {
        protected const int BUTTON_SIZE = 25;
        protected const int BOTTON_SPACING = 15;

        private ClickableSliderbar bar => (ClickableSliderbar)Control;

        protected override Drawable CreateControl() => new ClickableSliderbar
        {
            Margin = new MarginPadding { Top = 5, Bottom = 5 },
            RelativeSizeAxes = Axes.X
        };

        public ClickablePlayerSliderBar()
        {
            Padding = new MarginPadding { Left = BOTTON_SPACING * 2, Right = BOTTON_SPACING * 2 };
        }

        public void ResetToDefaultValue() => bar.ResetToDefaultValue();

        public void TriggerDecrease() => bar.TriggerDecrease();

        public void TriggerIncrease() => bar.TriggerIncrease();

        private class ClickableSliderbar : OsuSliderBar<int>
        {
            private readonly ToolTipButton decreaseButton;
            private readonly ToolTipButton increaseButton;

            public override string TooltipText => (Current.Value >= 0 ? "+" : "") + Current.Value.ToString("N0");

            public ClickableSliderbar()
            {
                KeyboardStep = 1;

                Add(decreaseButton = new ToolTipButton
                {
                    Position = new Vector2(-BOTTON_SPACING, 0),
                    Origin = Anchor.CentreRight,
                    Anchor = Anchor.CentreLeft,
                    Width = BUTTON_SIZE,
                    Height = BUTTON_SIZE,
                    Text = "-",
                    TooltipText = "Decrease",
                    Action = () => Current.Value -= (int)KeyboardStep
                });

                Add(increaseButton = new ToolTipButton
                {
                    Position = new Vector2(BOTTON_SPACING, 0),
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreRight,
                    Width = BUTTON_SIZE,
                    Height = BUTTON_SIZE,
                    Text = "+",
                    TooltipText = "Increase",
                    Action = () => Current.Value += (int)KeyboardStep
                });
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                AccentColour = colours.Yellow;
                Nub.AccentColour = colours.Yellow;
                Nub.GlowingAccentColour = colours.YellowLighter;
                Nub.GlowColour = colours.YellowDarker;
            }

            public void ResetToDefaultValue() => Current.SetDefault();

            public void TriggerDecrease() => decreaseButton.Action?.Invoke();

            public void TriggerIncrease() => increaseButton.Action?.Invoke();
        }

        private class ToolTipButton : TriangleButton, IHasTooltip
        {
            public string TooltipText { get; set; }
        }
    }
}
