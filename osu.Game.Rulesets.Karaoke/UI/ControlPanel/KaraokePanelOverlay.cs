// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Audio;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces;
using osuTK;
using osuTK.Graphics;
using ProgressBar = osu.Game.Rulesets.Karaoke.UI.ControlPanel.Pieces.ProgressBar;

namespace osu.Game.Rulesets.Karaoke.UI.ControlPanel
{
    public class KaraokePanelOverlay : Container, IKeyBindingHandler<KaraokeAction>
    {
        private const float content_width = 0.8f;
        private const int object_height = 30;
        private const int height = 130;
        private const int horizontal_conponent_spacing = 10;
        private const int margin_padding = 10;

        private readonly KaraokePlayfield karaokePlayfield;

        protected readonly WaveContainer Panel;
        protected readonly ToolTipButton FirstLyricButton;
        protected readonly ToolTipButton PreviousLyricButton;
        protected readonly ToolTipButton NextLyricButton;
        protected readonly PlayStateButton PlayPauseButton;
        protected readonly ProgressBar TimeSlideBar;
        protected readonly ButtonSlider TempoSlider;
        protected readonly ButtonSlider PitchSlider;
        protected readonly ButtonSlider LyricOffectSlider;

        public KaraokePanelOverlay(KaraokePlayfield playField = null)
        {
            karaokePlayfield = playField;

            Children = new Drawable[]
            {
                new ToolTipButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Margin = new MarginPadding(40),
                    Width = 90,
                    Height = 45,
                    Text = "Open/Close",
                    TooltipText = "Open/Close panel",
                    Action = ToggleVisibility
                },
                Panel = new WaveContainer
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = height,
                    FirstWaveColour = OsuColour.FromHex(@"19b0e2").Opacity(50),
                    SecondWaveColour = OsuColour.FromHex(@"2280a2").Opacity(50),
                    ThirdWaveColour = OsuColour.FromHex(@"005774").Opacity(50),
                    FourthWaveColour = OsuColour.FromHex(@"003a4e").Opacity(50),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Masking = true,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.Black.Opacity(0.4f)
                                }
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            Children = new Drawable[]
                            {
                                new Container
                                {
                                    Name = "Panel Container",
                                    Origin = Anchor.TopCentre,
                                    Anchor = Anchor.TopCentre,
                                    RelativeSizeAxes = Axes.X,
                                    Width = content_width,
                                    Height = height,
                                    Scale = new Vector2(1.0f),
                                    Padding = new MarginPadding(margin_padding),
                                    Children = new Drawable[]
                                    {
                                        new GridContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Content = new[]
                                            {
                                                new Drawable[]
                                                {
                                                    new Container
                                                    {
                                                        Name = "Time Section",
                                                        RelativeSizeAxes = Axes.Both,
                                                        Anchor = Anchor.CentreLeft,
                                                        Origin = Anchor.CentreLeft,
                                                        Children = new Drawable[]
                                                        {
                                                            new FillFlowContainer
                                                            {
                                                                Name = "Sentence",
                                                                Anchor = Anchor.CentreLeft,
                                                                Origin = Anchor.CentreLeft,
                                                                Direction = FillDirection.Horizontal,
                                                                Spacing = new Vector2(horizontal_conponent_spacing),
                                                                AutoSizeAxes = Axes.X,
                                                                Children = new Drawable[]
                                                                {
                                                                    // "Sentence" introduce text
                                                                    new TooltipSpriteText
                                                                    {
                                                                        Text = "Sentence",
                                                                        TooltipText = "Choose the sentence you want to sing."
                                                                    },

                                                                    // Switch to first sentence
                                                                    FirstLyricButton = new ToolTipButton
                                                                    {
                                                                        Origin = Anchor.CentreLeft,
                                                                        Width = object_height,
                                                                        Height = object_height,
                                                                        Text = "1",
                                                                        TooltipText = "Move to first sentence",
                                                                        Action = () => { karaokePlayfield?.NavigationToFirst(); }
                                                                    },

                                                                    // Switch to previous sentence
                                                                    PreviousLyricButton = new ToolTipButton
                                                                    {
                                                                        Origin = Anchor.CentreLeft,
                                                                        Width = object_height,
                                                                        Height = object_height,
                                                                        Text = "<-",
                                                                        TooltipText = "Move to previous sentence",
                                                                        Action = () => { karaokePlayfield?.NavigationToPrevious(); }
                                                                    },

                                                                    // Switch to next sentence
                                                                    NextLyricButton = new ToolTipButton
                                                                    {
                                                                        Origin = Anchor.CentreLeft,
                                                                        Width = object_height,
                                                                        Height = object_height,
                                                                        Text = "->",
                                                                        TooltipText = "Move to next sentence",
                                                                        Action = () => { karaokePlayfield?.NavigationToNext(); }
                                                                    },

                                                                    // "Play" introduce text
                                                                    new TooltipSpriteText
                                                                    {
                                                                        Text = "Play",
                                                                        TooltipText = "Pause,play the song and adjust time"
                                                                    },

                                                                    // Play and pause
                                                                    PlayPauseButton = new PlayStateButton
                                                                    {
                                                                        Origin = Anchor.CentreLeft,
                                                                        Width = object_height,
                                                                        Height = object_height,
                                                                        State = PlayState.Pause,
                                                                        Action = () =>
                                                                        {
                                                                            // TODO : move into PlayStateButton
                                                                            if (karaokePlayfield != null)
                                                                            {
                                                                                if (PlayPauseButton.State == PlayState.Pause)
                                                                                {
                                                                                    karaokePlayfield?.Pause();
                                                                                    PlayPauseButton.State = PlayState.Play;
                                                                                }
                                                                                else
                                                                                {
                                                                                    karaokePlayfield?.Play();
                                                                                    PlayPauseButton.State = PlayState.Pause;
                                                                                }
                                                                            }
                                                                        }
                                                                    },
                                                                }
                                                            },
                                                            new Container
                                                            {
                                                                Name = "Time Section",
                                                                RelativeSizeAxes = Axes.X,
                                                                Anchor = Anchor.CentreLeft,
                                                                Origin = Anchor.CentreLeft,
                                                                Padding = new MarginPadding { Left = 320, Right = 50 },
                                                                Children = new Drawable[]
                                                                {
                                                                    // Time slider
                                                                    TimeSlideBar = new ProgressBar
                                                                    {
                                                                        RelativeSizeAxes = Axes.X,
                                                                        Origin = Anchor.CentreLeft,
                                                                        StartTime = karaokePlayfield?.FirstObjectTime() ?? 0,
                                                                        EndTime = karaokePlayfield?.LastObjectTime() ?? 100000, //1:40
                                                                        OnSeek = newValue => { karaokePlayfield?.NavigateToTime(newValue); }
                                                                    },
                                                                }
                                                            }
                                                        }
                                                    },
                                                },
                                                new Drawable[]
                                                {
                                                    new GridContainer
                                                    {
                                                        RelativeSizeAxes = Axes.X,
                                                        Anchor = Anchor.CentreLeft,
                                                        Origin = Anchor.CentreLeft,
                                                        Content = new[]
                                                        {
                                                            new Drawable[]
                                                            {
                                                                new Container
                                                                {
                                                                    Name = "Tempo Section",
                                                                    RelativeSizeAxes = Axes.X,
                                                                    Anchor = Anchor.CentreLeft,
                                                                    Origin = Anchor.CentreLeft,
                                                                    Children = new Drawable[]
                                                                    {
                                                                        // "Tempo" introduce
                                                                        new TooltipSpriteText
                                                                        {
                                                                            Text = "Tempo",
                                                                            TooltipText = "Adjust song tempo."
                                                                        },

                                                                        new Container
                                                                        {
                                                                            RelativeSizeAxes = Axes.X,
                                                                            Padding = new MarginPadding { Left = 100, Right = 50 },
                                                                            Children = new Drawable[]
                                                                            {
                                                                                // Tempo
                                                                                TempoSlider = new ButtonSlider
                                                                                {
                                                                                    Origin = Anchor.CentreLeft,
                                                                                    RelativeSizeAxes = Axes.X,

                                                                                    MinValue = 0.5f,
                                                                                    MaxValue = 1.5f,
                                                                                    Value = 1,
                                                                                    DefauleValue = 1,
                                                                                    KeyboardStep = 0.05f,
                                                                                    Current = karaokePlayfield?.WorkingBeatmap.Track.Tempo ?? new Bindable<double>()
                                                                                },
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                new Container
                                                                {
                                                                    Name = "Pitch Section",
                                                                    RelativeSizeAxes = Axes.X,
                                                                    Anchor = Anchor.CentreLeft,
                                                                    Origin = Anchor.CentreLeft,
                                                                    Children = new Drawable[]
                                                                    {
                                                                        // "Pitch" introduce
                                                                        new TooltipSpriteText
                                                                        {
                                                                            Text = "Pitch",
                                                                            TooltipText = "Adjust song pitch."
                                                                        },

                                                                        new Container
                                                                        {
                                                                            RelativeSizeAxes = Axes.X,
                                                                            Padding = new MarginPadding { Left = 100, Right = 50 },
                                                                            Children = new Drawable[]
                                                                            {
                                                                                // Pitch
                                                                                PitchSlider = new ButtonSlider
                                                                                {
                                                                                    Origin = Anchor.CentreLeft,
                                                                                    RelativeSizeAxes = Axes.X,
                                                                                    MinValue = 0.5f,
                                                                                    MaxValue = 1.5f,
                                                                                    Value = 1.0f,
                                                                                    DefauleValue = 1,
                                                                                    KeyboardStep = 0.05f,
                                                                                    OnValueChanged = (eaa, newValue) =>
                                                                                    {
                                                                                        if (karaokePlayfield?.WorkingBeatmap.Track is IHasPitchAdjust pitchAdjust)
                                                                                            pitchAdjust.PitchAdjust = newValue;
                                                                                    }
                                                                                },
                                                                            }
                                                                        }
                                                                    }
                                                                },
                                                                new Container
                                                                {
                                                                    Name = "Offset Section",
                                                                    RelativeSizeAxes = Axes.X,
                                                                    Anchor = Anchor.CentreLeft,
                                                                    Origin = Anchor.CentreLeft,
                                                                    Children = new Drawable[]
                                                                    {
                                                                        // "Offset" introduce
                                                                        new TooltipSpriteText
                                                                        {
                                                                            Text = "Offset",
                                                                            TooltipText = "Adjust lyrics appear offset."
                                                                        },

                                                                        new Container
                                                                        {
                                                                            RelativeSizeAxes = Axes.X,
                                                                            Padding = new MarginPadding { Left = 100, Right = 50 },
                                                                            Children = new Drawable[]
                                                                            {
                                                                                // Offset
                                                                                LyricOffectSlider = new ButtonSlider
                                                                                {
                                                                                    Origin = Anchor.CentreLeft,
                                                                                    RelativeSizeAxes = Axes.X,
                                                                                    MinValue = -5.0f,
                                                                                    MaxValue = 5.0f,
                                                                                    Value = 0,
                                                                                    DefauleValue = 0,
                                                                                    KeyboardStep = 0.5f,
                                                                                    OnValueChanged = (eaa, newValue) => { karaokePlayfield?.AdjustlyricsOffset(newValue); }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public bool OnPressed(KaraokeAction action)
        {
            switch (action)
            {
                // Time
                case KaraokeAction.FirstLyric:
                    FirstLyricButton.Action?.Invoke();
                    break;

                case KaraokeAction.PreviousLyric:
                    PreviousLyricButton.Action?.Invoke();
                    break;

                case KaraokeAction.NextLyric:
                    NextLyricButton.Action?.Invoke();
                    break;

                case KaraokeAction.PlayAndPause:
                    PlayPauseButton.Action?.Invoke();
                    break;

                // Open panel
                case KaraokeAction.OpenPanel:
                    ToggleVisibility();
                    break;

                // Tempo
                case KaraokeAction.IncreaseTempo:
                    TempoSlider.TriggerIncrease();
                    break;

                case KaraokeAction.DecreaseTempo:
                    TempoSlider.TriggerDecrease();
                    break;

                case KaraokeAction.ResetTempo:
                    TempoSlider.ResetToDefauleValue();
                    break;

                // Pitch
                case KaraokeAction.IncreasePitch:
                    PitchSlider.TriggerIncrease();
                    break;

                case KaraokeAction.DecreasePitch:
                    PitchSlider.TriggerDecrease();
                    break;

                case KaraokeAction.ResetPitch:
                    PitchSlider.ResetToDefauleValue();
                    break;

                // Appear time
                case KaraokeAction.IncreaseLyricAppearTime:
                    LyricOffectSlider.TriggerIncrease();
                    break;

                case KaraokeAction.DecreaseLyricAppearTime:
                    LyricOffectSlider.TriggerDecrease();
                    break;

                case KaraokeAction.ResetLyricAppearTime:
                    LyricOffectSlider.ResetToDefauleValue();
                    break;
            }

            return false;
        }

        protected override void Update()
        {
            if (karaokePlayfield != null)
            {
                // Update current time
                var current = karaokePlayfield.GetCurrentTime();
                TimeSlideBar.CurrentTime = current;
            }
        }

        public void ToggleVisibility() => Panel.ToggleVisibility();

        public bool OnReleased(KaraokeAction action)
        {
            return true;
        }
    }
}
