// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Layout;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces
{
    public class DefaultBackgroundBodyPiece : Container
    {
        protected readonly Drawable Background;
        protected readonly Drawable Foreground;

        public DefaultBackgroundBodyPiece()
        {
            CornerRadius = DefaultBorderBodyPiece.CORNER_RADIUS;
            Masking = true;
            Height = DefaultColumnBackground.COLUMN_HEIGHT;

            Children = new[]
            {
                Background = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                Foreground = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            };

            AddLayout(subtractionCache);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            updateColour();
        }

        private Color4 accentColour;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                if (accentColour == value)
                    return;

                accentColour = value;
                updateColour();
            }
        }

        private Color4 hitColour;

        public Color4 HitColour
        {
            get => hitColour;
            set
            {
                if (hitColour == value)
                    return;

                hitColour = value;
                updateColour();
            }
        }

        private bool hitting;

        public bool Hitting
        {
            get => hitting;
            set
            {
                if (hitting == value)
                    return;

                hitting = value;
                updateColour();
            }
        }

        private bool display;

        public bool Display
        {
            get => display;
            set
            {
                if (display == value)
                    return;

                display = value;
                updateColour();
            }
        }

        private readonly LayoutValue subtractionCache = new LayoutValue(Invalidation.DrawSize);

        private void updateColour()
        {
            if (!IsLoaded)
                return;

            Foreground.Colour = HitColour;
            Background.Colour = Display ? AccentColour : new Color4(23, 41, 46, 255);

            Foreground.ClearTransforms(false, nameof(Foreground.Colour));
            Foreground.Alpha = 0;

            if (hitting)
            {
                Foreground.Alpha = 1;

                const float animation_length = 50;

                // wait for the next sync point
                double synchronisedOffset = animation_length * 2 - Time.Current % (animation_length * 2);
                using (Foreground.BeginDelayedSequence(synchronisedOffset))
                    Foreground.FadeColour(HitColour.Lighten(0.7f), animation_length).Then().FadeColour(Foreground.Colour, animation_length).Loop();
            }

            subtractionCache.Invalidate();
        }
    }
}
