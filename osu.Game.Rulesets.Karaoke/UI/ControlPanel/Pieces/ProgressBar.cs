// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Threading;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces
{
    public class ProgressBar : OsuSliderBar<double>
    {
        public override string TooltipText => GetTimeFormat((int)CurrentNumber.Value / 1000);

        public Action<double> OnSeek;

        protected OsuSpriteText NowTimeSpriteText;
        protected OsuSpriteText TotalTimeSpriteText;

        public double StartTime
        {
            set => CurrentNumber.MinValue = value;
        }

        public double EndTime
        {
            set
            {
                CurrentNumber.MaxValue = value;
                TotalTimeSpriteText.Text = GetTimeFormat(((int)CurrentNumber.MaxValue - (int)CurrentNumber.MinValue) / 1000);
            }
        }

        public double CurrentTime
        {
            get => CurrentNumber.Value;
            set
            {
                CurrentNumber.Value = value;
                NowTimeSpriteText.Text = GetTimeFormat(((int)CurrentNumber.Value - (int)CurrentNumber.MinValue) / 1000);
            }
        }

        public ProgressBar()
        {
            CurrentNumber.MinValue = 0;
            CurrentNumber.MaxValue = 1;
            KeyboardStep = 1000f;

            // Now time
            Add(NowTimeSpriteText = new OsuSpriteText
            {
                Position = new Vector2(-10, -2),
                Text = "--:--",
                UseFullGlyphHeight = false,
                Origin = Anchor.CentreRight,
                Anchor = Anchor.CentreLeft,
                Font = new FontUsage(size: 15),
                Alpha = 1
            });

            // End time
            Add(TotalTimeSpriteText = new OsuSpriteText
            {
                Position = new Vector2(35, -2),
                Text = "--:--",
                UseFullGlyphHeight = false,
                Origin = Anchor.CentreRight,
                Anchor = Anchor.CentreRight,
                Font = new FontUsage(size: 15),
                Alpha = 1
            });
        }

        private ScheduledDelegate scheduledSeek;

        protected override void OnUserChange(double value)
        {
            scheduledSeek?.Cancel();
            scheduledSeek = Schedule(() => OnSeek?.Invoke(value));
        }

        protected string GetTimeFormat(int second)
        {
            return (second / 60).ToString("D2") + ":" + (second % 60).ToString("D2");
        }
    }
}
