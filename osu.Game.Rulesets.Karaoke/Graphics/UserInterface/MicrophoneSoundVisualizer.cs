// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
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
        private const float max_decibel = 100;
        private const float max_pitch = 60;

        private readonly Box background;
        private readonly MicrophoneInfo microphoneInfo;
        private readonly DecibelVisualizer decibelVisualizer;
        private readonly PitchVisualizer pitchVisualizer;

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
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.6f),
                        new Dimension(GridSizeMode.Relative, 0.2f),
                        new Dimension(GridSizeMode.Relative, 0.2f),
                    },
                    Content = new[]
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
                            decibelVisualizer = new DecibelVisualizer
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Y = 2,
                            },
                        },
                        new Drawable[]
                        {
                            pitchVisualizer = new PitchVisualizer
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                            },
                        }
                    }
                }
            };

            updateDeviceInfo();
        }

        private string deviceName;

        public string DeviceName
        {
            get => deviceName;
            set
            {
                if (deviceName == value)
                    return;

                deviceName = value;
                updateDeviceInfo();
            }
        }

        private bool hasDevice;

        public bool HasDevice
        {
            get => hasDevice;
            set
            {
                if (hasDevice == value)
                    return;

                hasDevice = value;
                updateDeviceInfo();
            }
        }

        private void updateDeviceInfo()
        {
            microphoneInfo.DeviceName = HasDevice ? DeviceName : "Seems no microphone device.";
            microphoneInfo.HasDevice = HasDevice;
        }

        protected override bool Handle(UIEvent e)
        {
            return e switch
            {
                MicrophoneStartPitchingEvent microphoneStartPitching => OnMicrophoneStartSinging(microphoneStartPitching),
                MicrophoneEndPitchingEvent microphoneEndPitching => OnMicrophoneEndSinging(microphoneEndPitching),
                MicrophonePitchingEvent microphonePitching => OnMicrophoneSinging(microphonePitching),
                _ => base.Handle(e)
            };
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
            decibelVisualizer.Decibel = 0;
            pitchVisualizer.Pitch = 0;

            return false;
        }

        protected virtual bool OnMicrophoneSinging(MicrophonePitchingEvent e)
        {
            var voice = e.CurrentState.Microphone.Voice;
            float decibel = voice.Decibel;
            float pitch = voice.Pitch;

            // todo : should convert to better value.
            decibelVisualizer.Decibel = decibel;
            pitchVisualizer.Pitch = pitch / 8;

            return false;
        }

        internal class MicrophoneInfo : CompositeDrawable
        {
            private readonly Box background;
            private readonly SpriteIcon microphoneIcon;
            private readonly OsuSpriteText deviceName;

            [Resolved]
            private OsuColour colours { get; set; }

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
                        Children = new Drawable[]
                        {
                            microphoneIcon = new SpriteIcon
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
                set => deviceName.Text = value;
            }

            public bool HasDevice
            {
                set =>
                    Schedule(() =>
                    {
                        microphoneIcon.Colour = value ? colours.GrayF : colours.RedLight;
                    });
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray3;
            }
        }

        internal class DecibelVisualizer : CompositeDrawable
        {
            private const float var_width = 294;

            private readonly Box background;
            private readonly Box decibelMarker;
            private readonly Box decibelRippleMarker;
            private readonly Box maxDecibelMarker;

            public DecibelVisualizer()
            {
                Width = var_width;
                Height = 8;
                InternalChildren = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    decibelMarker = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                    },
                    decibelRippleMarker = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                    },
                    maxDecibelMarker = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 5,
                    },
                };
            }

            private float decibel;

            private float maxDecibel;

            public float Decibel
            {
                get => decibel;
                set
                {
                    if (decibel == value)
                        return;

                    decibel = value;
                    if (decibel > maxDecibel)
                        maxDecibel = decibel;

                    if (decibel > rippleDecibel)
                        rippleDecibel = value;

                    decibelMarker.Width = calculatePosition(Decibel);
                    maxDecibelMarker.X = calculatePosition(maxDecibel);
                }
            }

            private float rippleDecibel;

            protected override void Update()
            {
                base.Update();

                if (rippleDecibel <= 0)
                    return;

                //1% of extra bar length to make it a little faster when bar is almost at it's minimum
                rippleDecibel *= 0.99f;

                // just make value to 0 if too small;
                if (rippleDecibel < 0.5)
                    rippleDecibel = 0;

                decibelRippleMarker.Width = rippleDecibel;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray5;
                decibelMarker.Colour = colours.GrayD;
                decibelRippleMarker.Colour = colours.GrayA;
                maxDecibelMarker.Colour = colours.Red;
            }

            private float calculatePosition(float decibel)
                => decibel / max_decibel * var_width;
        }

        internal class PitchVisualizer : CompositeDrawable
        {
            private const int dot_width = 5;
            private const int dot_height = 10;
            private const int dot_amount = 30;
            private const float spacing = 5;

            private readonly PitchDot currentDot;

            public PitchVisualizer()
            {
                AutoSizeAxes = Axes.Both;

                // todo : draw that stupid shapes with progressive background color.
                InternalChildren = new Drawable[]
                {
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(spacing),
                        Children = Enumerable.Range(0, dot_amount).Select(i => new PitchDot
                        {
                            Colour = calculateDotColour(i, 0.8f)
                        }).ToArray()
                    },
                    currentDot = new PitchDot
                    {
                        Alpha = 0,
                    },
                };
            }

            private float pitch;

            private bool showPitch;

            public float Pitch
            {
                get => pitch;
                set
                {
                    if (EqualityComparer<float>.Default.Equals(pitch, value))
                        return;

                    pitch = value;

                    // adjust dot position
                    currentDot.X = calculateDotPosition((int)pitch);

                    // adjust show / hide.
                    bool show = pitch != 0;
                    if (showPitch == show)
                        return;

                    showPitch = show;

                    if (show)
                    {
                        currentDot.FadeIn(200);
                    }
                    else
                    {
                        currentDot.FadeOut(200);
                    }
                }
            }

            private Color4 calculateDotColour(int index, float s)
            {
                const float start_v = 0.4f;
                const float end_v = 0.7f;
                float v = (end_v - start_v) / dot_amount * index + start_v;
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
