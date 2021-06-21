// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class CircleCheckbox : Checkbox, IHasAccentColour
    {
        public const float EXPANDED_SIZE = 24;

        private readonly Circle background;
        private readonly SpriteIcon border;
        private readonly SpriteIcon selectedIcon;

        private Sample sampleChecked;
        private Sample sampleUnchecked;

        /// <summary>
        /// Whether to play sounds when the state changes as a result of user interaction.
        /// </summary>
        protected virtual bool PlaySoundsOnUserChange => true;

        public CircleCheckbox()
        {
            Size = new Vector2(EXPANDED_SIZE);

            Children = new Drawable[]
            {
                background = new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.5f,
                },
                border = new SpriteIcon
                {
                    RelativeSizeAxes = Axes.Both,
                    Icon = FontAwesome.Regular.Circle,
                },
                selectedIcon = new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Icon = FontAwesome.Solid.Check,
                    Scale = new Vector2(0),
                },
                new HoverSounds()
            };

            Current.DisabledChanged += disabled => background.Alpha = selectedIcon.Alpha = disabled ? 0.3f : 1;

            Current.ValueChanged += (e) =>
            {
                selectedIcon.ScaleTo(e.NewValue ? 0.6f : 0, 200, Easing.OutElastic);
            };
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            sampleChecked = audio.Samples.Get(@"UI/check-on");
            sampleUnchecked = audio.Samples.Get(@"UI/check-off");
        }

        private Color4 accentColour;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                accentColour = value;

                background.Colour = AccentColour.Darken(1.5f);
                border.Colour = AccentColour;
                selectedIcon.Colour = AccentColour;
            }
        }

        protected override void OnUserChange(bool value)
        {
            base.OnUserChange(value);

            if (PlaySoundsOnUserChange)
            {
                if (value)
                    sampleChecked?.Play();
                else
                    sampleUnchecked?.Play();
            }
        }
    }
}
