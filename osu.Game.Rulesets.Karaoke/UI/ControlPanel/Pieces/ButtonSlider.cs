// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces
{
    public class ButtonSlider : OsuSliderBar<double>
    {
        protected const int BUTTON_SIZE = 25;

        public override string TooltipText => Current.Value.ToString(@"0.##");

        public EventHandler<double> OnValueChanged;

        protected ToolTipButton DecreaseButton;
        protected ToolTipButton IncreaseButton;

        public double MinValue
        {
            get => CurrentNumber.MinValue;
            set => CurrentNumber.MinValue = value;
        }

        public double MaxValue
        {
            get => CurrentNumber.MaxValue;
            set => CurrentNumber.MaxValue = value;
        }

        public double Value
        {
            get => CurrentNumber.Value;
            set => CurrentNumber.Value = value;
        }

        public float DefauleValue { get; set; }

        public ButtonSlider()
        {
            CurrentNumber.MinValue = 0;
            CurrentNumber.MaxValue = 1;
            KeyboardStep = 0.1f;

            Add(DecreaseButton = new ToolTipButton
            {
                Position = new Vector2(-10, 0),
                Origin = Anchor.CentreRight,
                Anchor = Anchor.CentreLeft,
                Width = BUTTON_SIZE,
                Height = BUTTON_SIZE,
                Text = "-",
                TooltipText = "Decrease",
                Action = () =>
                {
                    var newValue = Value - KeyboardStep;
                    Value = newValue;
                }
            });

            Add(IncreaseButton = new ToolTipButton
            {
                Position = new Vector2(10, 0),
                Origin = Anchor.CentreLeft,
                Anchor = Anchor.CentreRight,
                Width = BUTTON_SIZE,
                Height = BUTTON_SIZE,
                Text = "+",
                TooltipText = "Increase",
                Action = () =>
                {
                    var newValue = Value + KeyboardStep;
                    Value = newValue;
                }
            });
        }

        public void ResetToDefauleValue()
        {
            Value = DefauleValue;
        }

        public void TriggerDecrease() => DecreaseButton.Action?.Invoke();

        public void TriggerIncrease() => IncreaseButton.Action?.Invoke();

        protected override void UpdateValue(float value)
        {
            base.UpdateValue(value);
            OnValueChanged?.Invoke(this, Value);
        }
    }
}
