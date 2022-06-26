// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class ClickablePlayerSliderBar : SettingsSlider<int>
    {
        protected const int BUTTON_SIZE = 25;
        protected const int BUTTON_SPACING = 15;

        private ClickableSliderBar bar => (ClickableSliderBar)Control;

        protected override Drawable CreateControl() => new ClickableSliderBar
        {
            Margin = new MarginPadding { Top = 5, Bottom = 5 },
            RelativeSizeAxes = Axes.X
        };

        public ClickablePlayerSliderBar()
        {
            Padding = new MarginPadding { Left = BUTTON_SPACING * 2, Right = BUTTON_SPACING * 2 };
        }

        public void ResetToDefaultValue() => bar.ResetToDefaultValue();

        public void TriggerDecrease() => bar.TriggerDecrease();

        public void TriggerIncrease() => bar.TriggerIncrease();

        private class ClickableSliderBar : OsuSliderBar<int>
        {
            private readonly ToolTipButton decreaseButton;
            private readonly ToolTipButton increaseButton;

            public override LocalisableString TooltipText => (Current.Value >= 0 ? "+" : string.Empty) + Current.Value.ToString("N0");

            public ClickableSliderBar()
            {
                KeyboardStep = 1;

                Add(decreaseButton = new ToolTipButton
                {
                    Position = new Vector2(-BUTTON_SPACING, 0),
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
                    Position = new Vector2(BUTTON_SPACING, 0),
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

        private class ToolTipButton : OsuButton, IHasTooltip
        {
            public LocalisableString TooltipText { get; set; }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                BackgroundColour = colours.Blue;
            }
        }
    }
}
