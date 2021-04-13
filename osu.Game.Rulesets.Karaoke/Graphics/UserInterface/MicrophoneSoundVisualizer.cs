// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterface
{
    public class MicrophoneSoundVisualizer : CompositeDrawable
    {
        private static float max_loudness = 100;
        private static float max_pitch = 60;

        private readonly Box background;
        private readonly MicrophoneInfo microphoneInfo;
        private readonly LoudnessVisualizer loudnessVisualizer;
        private readonly PitchVisualier pitchVisualier;

        public MicrophoneSoundVisualizer()
        {
            Width = 310;
            Height = 100;
            Masking = true;
            CornerRadius = 5f;
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new []
                    {
                        new Dimension(GridSizeMode.Relative, 0.6f),
                        new Dimension(GridSizeMode.Relative, 0.2f),
                        new Dimension(GridSizeMode.Relative, 0.2f),
                    },
                    Content = new []
                    {
                        new Drawable[]
                        {
                            microphoneInfo = new MicrophoneInfo
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                        },
                        new Drawable[]
                        {
                            loudnessVisualizer = new LoudnessVisualizer
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Y = 2,
                            },
                        },
                        new Drawable[]
                        {
                            pitchVisualier = new PitchVisualier
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                            },
                        }
                    }
                }
            };
        }

        public string DeviceName
        {
            get => microphoneInfo.DeviceName;
            set => microphoneInfo.DeviceName = value;
        }

        protected override bool Handle(UIEvent e)
        {
            switch (e)
            {
                case MicrophoneStartPitchingEvent microphoneStartPitching:
                    return OnMicrophoneStartSinging(microphoneStartPitching);

                case MicrophoneEndPitchingEvent microphoneEndPitching:
                    return OnMicrophoneEndSinging(microphoneEndPitching);

                case MicrophonePitchingEvent microphonePitching:
                    return OnMicrophoneSinging(microphonePitching);

                default:
                    return base.Handle(e);
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.Gray2;
        }

        protected virtual bool OnMicrophoneStartSinging(MicrophoneStartPitchingEvent e)
        {
            return false;
        }

        protected virtual bool OnMicrophoneEndSinging(MicrophoneEndPitchingEvent e)
        {
            loudnessVisualizer.Loudness = 0;
            pitchVisualier.Pitch = 0;

            return false;
        }

        protected virtual bool OnMicrophoneSinging(MicrophonePitchingEvent e)
        {
            var loudness = e.CurrentState.Microphone.Loudness;
            var pitch = e.CurrentState.Microphone.Pitch;

            // todo : should convert to better value.
            loudnessVisualizer.Loudness = loudness;
            pitchVisualier.Pitch = pitch / 8;

            return false;
        }

        internal class MicrophoneInfo : CompositeDrawable
        {
            private readonly Box background;
            private readonly OsuSpriteText deviceName;

            public MicrophoneInfo()
            {
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding
                        {
                            Top = 20,
                            Left = 15
                        },
                        Spacing = new Vector2(15),
                        Direction = FillDirection.Horizontal,
                        Children = new Drawable []
                        {
                            new SpriteIcon
                            {
                                Size = new Vector2(24),
                                Icon = FontAwesome.Solid.Microphone
                            },
                            deviceName = new OsuSpriteText
                            {
                                Width = 250,
                                Font = OsuFont.Default.With(size: 20),
                                Text = "Microphone name",
                                Truncate = true,
                            }
                        }
                    }
                };
            }

            public string DeviceName
            {
                get => deviceName.Text.ToString();
                set => deviceName.Text = value;
            }


            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray3;
            }
        }

        internal class LoudnessVisualizer : CompositeDrawable
        {
            private const float var_width = 294;

            private readonly Box background;
            private readonly Box loudnessMarker;
            private readonly Box loudnessRippleMarker;
            private readonly Box maxLoudnessMarker;

            public LoudnessVisualizer()
            {
                Width = var_width;
                Height = 8;
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    loudnessMarker = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                    },
                    loudnessRippleMarker = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                    },
                    maxLoudnessMarker = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 5,
                    },
                };
            }

            private float loudness;

            private float maxLoudness;

            public float Loudness
            {
                get => loudness;
                set
                {
                    if (loudness == value)
                        return;

                    loudness = value;
                    if (loudness > maxLoudness)
                        maxLoudness = loudness;

                    loudnessMarker.Width = calculatePosition(Loudness);
                    maxLoudnessMarker.X = calculatePosition(maxLoudness);
                }
            }

            private float rippleLoudness;

            protected override void Update()
            {
                base.Update();

                // todo : celculate ripple loudness
                loudnessRippleMarker.Width = rippleLoudness;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray5;
                maxLoudnessMarker.Colour = colours.Red;
            }

            private float calculatePosition(float loudness)
                => loudness / max_loudness * var_width;
        }

        internal class PitchVisualier : CompositeDrawable
        {
            private const int dot_width = 5;
            private const int dot_height = 10;
            private const int dot_amount = 30;
            private const float spacing = 5;

            private readonly FillFlowContainer dots;
            private readonly PitchDot currentDot;

            public PitchVisualier()
            {
                // todo : draw that stupid shapes with progressive background color.
                AutoSizeAxes = Axes.Both;
                InternalChildren = new Drawable[]
                {
                    dots = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(spacing),
                    },
                    currentDot = new PitchDot
                    {
                        Alpha = 0,
                    },
                };

                for (int i = 0; i < dot_amount; i++)
                {
                    dots.Add(new PitchDot
                    {
                        Colour = calcualteDotColour(i, 0.8f)
                    });
                }
            }

            private float pitch;

            private bool showPitch;

            public float Pitch
            {
                get => pitch;
                set
                {
                    if (pitch == value)
                        return;

                    pitch = value;

                    // adjust dot position
                    currentDot.X = calculateDotPosition((int)pitch);

                    // adjust show / hide.
                    var showPitch = pitch != 0;
                    if (this.showPitch == showPitch)
                        return;

                    this.showPitch = showPitch;
                    if (showPitch)
                    {
                        currentDot.FadeIn(200);
                    }
                    else
                    {
                        currentDot.FadeOut(200);
                    }
                }
            }

            private Color4 calcualteDotColour(int index, float s)
            {
                const float start_v = 0.4f;
                const float end_v = 0.7f;
                var v = (end_v - start_v) / dot_amount * index + start_v;
                return Color4Extensions.FromHSV(0, s, v);
            }

            private float calculateDotPosition(int index)
                => (dot_width + spacing) * index;

            public class PitchDot : Container
            {
                private readonly CircularContainer circle;

                public PitchDot()
                {
                    Size = new Vector2(dot_width, dot_height);
                    Children = new[]
                    {
                        circle = new CircularContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Masking = true,
                            Alpha = 0,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.8f, 0),
                            Children = new[]
                            {
                                new Box
                                {
                                    Colour = Color4.White,
                                    RelativeSizeAxes = Axes.Both,
                                }
                            },
                        }
                    };
                }

                protected override void LoadComplete()
                {
                    base.LoadComplete();
                    circle.FadeIn(500, Easing.OutQuint);
                    circle.ResizeTo(new Vector2(0.8f), 500, Easing.OutQuint);
                }
            }
        }
    }
}
