// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Threading;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Input.Bindings;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;
using osu.Game.Rulesets.Karaoke.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModPractice : ModNoFail, IApplicableToDrawableRuleset<KaraokeHitObject>, IApplicableToHUD, IApplicableToTrack, IApplicableToBeatmap
    {
        public override string Name => "Practice";
        public override string Acronym => "Practice";
        public override double ScoreMultiplier => 0.0f;
        public override IconUsage Icon => FontAwesome.Solid.Adjust;

        private RulesetInfo rulesetInfo;
        private KaraokeBeatmap beatmap;

        private HUDOverlay overlay;

        public void ApplyToBeatmap(IBeatmap beatmap) => this.beatmap = beatmap as KaraokeBeatmap;

        public void ApplyToDrawableRuleset(DrawableRuleset<KaraokeHitObject> drawableRuleset)
        {
            beatmap = drawableRuleset.Beatmap as KaraokeBeatmap;
            rulesetInfo = drawableRuleset.Ruleset.RulesetInfo;

            if(drawableRuleset.Playfield is KaraokePlayfield karaokePlayfield)
                karaokePlayfield.DisplayCursor = new BindableBool
                {
                    Default = true,
                    Value = true
                };
        }

        public void ApplyToTrack(Track track)
        {
            // Create overlay
            overlay?.Add(new KarokeActionContainer(rulesetInfo)
            {
                RelativeSizeAxes = Axes.Both,
                Child = new KaraokePanelOverlay(beatmap,track)
                {
                    Clock = new FramedClock(new StopwatchClock(true)),
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        public new void ApplyToHUD(HUDOverlay overlay)
        {
            this.overlay = overlay;
        }        

        public class KarokeActionContainer : DatabasedKeyBindingContainer<KaraokeAction>
        {
            public KarokeActionContainer(RulesetInfo ruleset)
                : base(ruleset, 0, SimultaneousBindingMode.Unique)
            {
            }
        }

        public class KaraokePanelOverlay : Container, IKeyBindingHandler<KaraokeAction>
        {
            private const float content_width = 0.8f;
            private const int object_height = 30;
            private const int height = 130;
            private const int horizontal_conponent_spacing = 10;
            private const int margin_padding = 10;

            private List<LyricLine> lyrics => beatmap?.HitObjects.OfType<LyricLine>().ToList();
            private readonly KaraokeBeatmap beatmap;

            private readonly Track track;

            private readonly WaveContainer panel;
            private readonly ToolTipButton firstLyricButton;
            private readonly ToolTipButton previousLyricButton;
            private readonly ToolTipButton nextLyricButton;
            private readonly PlayStateButton playPauseButton;
            private readonly ProgressBar timeSlideBar;
            private readonly ButtonSlider tempoSlider;
            private readonly ButtonSlider pitchSlider;
            private readonly ButtonSlider lyricOffectSlider;

            public KaraokePanelOverlay(KaraokeBeatmap beatmap,Track track)
            {
                this.beatmap = beatmap;
                this.track = track;

                Children = new Drawable[]
                {
                    new ToolTipButton
                    {
                        Name = "Open/close panel button.",
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Margin = new MarginPadding(40),
                        Width = 90,
                        Height = 45,
                        Text = "Open/Close",
                        TooltipText = "Open/Close panel",
                        Action = ToggleVisibility
                    },
                    panel = new WaveContainer
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
                            new Box
                            {
                                Name = "Background",
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Black.Opacity(0.4f)
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
                                                                    Name = "Lyric section",
                                                                    Anchor = Anchor.CentreLeft,
                                                                    Origin = Anchor.CentreLeft,
                                                                    Direction = FillDirection.Horizontal,
                                                                    Spacing = new Vector2(horizontal_conponent_spacing),
                                                                    AutoSizeAxes = Axes.X,
                                                                    Children = new Drawable[]
                                                                    {
                                                                        new TooltipSpriteText
                                                                        {
                                                                            Text = "Lyric",
                                                                            TooltipText = "Choose the lyric you want to sing."
                                                                        },
                                                                        firstLyricButton = new ToolTipButton
                                                                        {
                                                                            Name = "First lyric button",
                                                                            Origin = Anchor.CentreLeft,
                                                                            Width = object_height,
                                                                            Height = object_height,
                                                                            Text = "1",
                                                                            TooltipText = "Move to first lyric",
                                                                            Action = () =>
                                                                            {
                                                                                var firstObject = lyrics.FirstOrDefault();
                                                                                if (firstObject!=null)
                                                                                    track?.Seek(firstObject.StartTime);
                                                                            }
                                                                        },
                                                                        previousLyricButton = new ToolTipButton
                                                                        {
                                                                            Name = "Pervious lyric button",
                                                                            Origin = Anchor.CentreLeft,
                                                                            Width = object_height,
                                                                            Height = object_height,
                                                                            Text = "<-",
                                                                            TooltipText = "Move to previous lyric",
                                                                            Action = () =>
                                                                            {
                                                                                if (track == null)
                                                                                    return;

                                                                                var nextLyric = lyrics.FirstOrDefault(x => x.StartTime > track.CurrentTime);
                                                                                if (nextLyric != null)
                                                                                    return;

                                                                                var perviousLyric = lyrics.GetPrevious(nextLyric);
                                                                                if(perviousLyric!=null)
                                                                                    track?.Seek(perviousLyric.StartTime);
                                                                            }
                                                                        },
                                                                        nextLyricButton = new ToolTipButton
                                                                        {
                                                                            Name = "Next lyric button",
                                                                            Origin = Anchor.CentreLeft,
                                                                            Width = object_height,
                                                                            Height = object_height,
                                                                            Text = "->",
                                                                            TooltipText = "Move to next lyric",
                                                                            Action = () =>
                                                                            {
                                                                                if (track == null)
                                                                                    return;

                                                                                var nextLyric = lyrics.FirstOrDefault(x => x.StartTime > track.CurrentTime);
                                                                                if (nextLyric != null)
                                                                                    track?.Seek(nextLyric.StartTime);
                                                                            }
                                                                        },
                                                                        new TooltipSpriteText
                                                                        {
                                                                            Text = "Play",
                                                                            TooltipText = "Pause,play the song and adjust time"
                                                                        },
                                                                        playPauseButton = new PlayStateButton
                                                                        {
                                                                            Name = "Play button",
                                                                            Origin = Anchor.CentreLeft,
                                                                            Width = object_height,
                                                                            Height = object_height,
                                                                            State = PlayState.Pause,
                                                                            Action = () =>
                                                                            {
                                                                                if (playPauseButton.State == PlayState.Pause)
                                                                                {
                                                                                    track?.Stop();
                                                                                    playPauseButton.State = PlayState.Play;
                                                                                }
                                                                                else
                                                                                {
                                                                                    track?.Start();
                                                                                    playPauseButton.State = PlayState.Pause;
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
                                                                        timeSlideBar = new ProgressBar
                                                                        {
                                                                            Name = "Time silder",
                                                                            RelativeSizeAxes = Axes.X,
                                                                            Origin = Anchor.CentreLeft,
                                                                            StartTime = lyrics?.FirstOrDefault()?.StartTime ?? 0,
                                                                            EndTime = lyrics?.LastOrDefault()?.EndTime ?? 100000, //1:40
                                                                            OnSeek = newValue => track?.Seek(newValue)
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
                                                                                    tempoSlider = new ButtonSlider
                                                                                    {
                                                                                        Name = "Tempo slider",
                                                                                        Origin = Anchor.CentreLeft,
                                                                                        RelativeSizeAxes = Axes.X,
                                                                                        DefauleValue = 1,
                                                                                        KeyboardStep = 0.05f,
                                                                                        Current = new BindableNumber<double>
                                                                                        {
                                                                                            MinValue = 0.5f,
                                                                                            MaxValue = 1.5f,
                                                                                            Default = 1,
                                                                                            Value = 1
                                                                                        }
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
                                                                                    pitchSlider = new ButtonSlider
                                                                                    {
                                                                                        Name = "Pitch slider",
                                                                                        Origin = Anchor.CentreLeft,
                                                                                        RelativeSizeAxes = Axes.X,
                                                                                        DefauleValue = 1,
                                                                                        KeyboardStep = 0.05f,
                                                                                        Current = new BindableNumber<double>
                                                                                        {
                                                                                            MinValue = 0.5f,
                                                                                            MaxValue = 1.5f,
                                                                                            Default = 1.0f,
                                                                                            Value = 1.0f
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
                                                                                    lyricOffectSlider = new ButtonSlider
                                                                                    {
                                                                                        Name = "Offset slider",
                                                                                        Origin = Anchor.CentreLeft,
                                                                                        RelativeSizeAxes = Axes.X,
                                                                                        DefauleValue = 0,
                                                                                        KeyboardStep = 0.5f,
                                                                                        Current = new BindableNumber<double>
                                                                                        {
                                                                                            MinValue = -5.0f,
                                                                                            MaxValue = 5.0f,
                                                                                            Value = 0,
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
                                                }
                                            },
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                tempoSlider.Current.BindValueChanged(value =>
                {
                    if (track != null)
                        track.Tempo.Value = value.NewValue;
                });

                pitchSlider.Current.BindValueChanged(value =>
                {
                    if (track != null)
                        track.Frequency.Value = value.NewValue;
                });

                lyricOffectSlider.Current.BindValueChanged(value =>
                {
                    // TODO : adjust offset
                });
            }

            public bool OnPressed(KaraokeAction action)
            {
                switch (action)
                {
                    // Time
                    case KaraokeAction.FirstLyric:
                        firstLyricButton.Action?.Invoke();
                        break;

                    case KaraokeAction.PreviousLyric:
                        previousLyricButton.Action?.Invoke();
                        break;

                    case KaraokeAction.NextLyric:
                        nextLyricButton.Action?.Invoke();
                        break;

                    case KaraokeAction.PlayAndPause:
                        playPauseButton.Action?.Invoke();
                        break;

                    // Open panel
                    case KaraokeAction.OpenPanel:
                        ToggleVisibility();
                        break;

                    // Tempo
                    case KaraokeAction.IncreaseTempo:
                        tempoSlider.TriggerIncrease();
                        break;

                    case KaraokeAction.DecreaseTempo:
                        tempoSlider.TriggerDecrease();
                        break;

                    case KaraokeAction.ResetTempo:
                        tempoSlider.ResetToDefauleValue();
                        break;

                    // Pitch
                    case KaraokeAction.IncreasePitch:
                        pitchSlider.TriggerIncrease();
                        break;

                    case KaraokeAction.DecreasePitch:
                        pitchSlider.TriggerDecrease();
                        break;

                    case KaraokeAction.ResetPitch:
                        pitchSlider.ResetToDefauleValue();
                        break;

                    // Appear time
                    case KaraokeAction.IncreaseLyricAppearTime:
                        lyricOffectSlider.TriggerIncrease();
                        break;

                    case KaraokeAction.DecreaseLyricAppearTime:
                        lyricOffectSlider.TriggerDecrease();
                        break;

                    case KaraokeAction.ResetLyricAppearTime:
                        lyricOffectSlider.ResetToDefauleValue();
                        break;
                }

                return false;
            }

            protected override void Update()
            {
                // Update current time
                if (track != null)
                    timeSlideBar.CurrentTime = track.CurrentTime;
            }

            public void ToggleVisibility() => panel.ToggleVisibility();

            public bool OnReleased(KaraokeAction action)
            {
                return true;
            }

            public class ButtonSlider : OsuSliderBar<double>
            {
                protected const int BUTTON_SIZE = 25;

                public override string TooltipText => Current.Value.ToString(@"0.##");

                private readonly ToolTipButton decreaseButton;
                private readonly ToolTipButton increaseButton;

                public float DefauleValue { get; set; }

                public ButtonSlider()
                {
                    KeyboardStep = 0.1f;

                    Add(decreaseButton = new ToolTipButton
                    {
                        Position = new Vector2(-10, 0),
                        Origin = Anchor.CentreRight,
                        Anchor = Anchor.CentreLeft,
                        Width = BUTTON_SIZE,
                        Height = BUTTON_SIZE,
                        Text = "-",
                        TooltipText = "Decrease",
                        Action = () => Current.Value -= KeyboardStep
                    });

                    Add(increaseButton = new ToolTipButton
                    {
                        Position = new Vector2(10, 0),
                        Origin = Anchor.CentreLeft,
                        Anchor = Anchor.CentreRight,
                        Width = BUTTON_SIZE,
                        Height = BUTTON_SIZE,
                        Text = "+",
                        TooltipText = "Increase",
                        Action = () => Current.Value += KeyboardStep
                    });
                }

                public void ResetToDefauleValue() => Current.SetDefault();

                public void TriggerDecrease() => decreaseButton.Action?.Invoke();

                public void TriggerIncrease() => increaseButton.Action?.Invoke();
            }

            public class PlayStateButton : IconButton
            {
                private PlayState state;

                /// <summary>
                /// If paused , show pause icon
                /// </summary>
                public PlayState State
                {
                    set
                    {
                        state = value;

                        switch (value)
                        {
                            case PlayState.Play:
                                Icon = FontAwesome.Regular.PlayCircle;
                                TooltipText = "Play";
                                break;

                            case PlayState.Pause:
                                Icon = FontAwesome.Regular.PauseCircle;
                                TooltipText = "Pause";
                                break;
                        }
                    }
                    get => state;
                }

                public PlayStateButton()
                {
                    Masking = true;
                    CornerRadius = 5;
                }

                [BackgroundDependencyLoader]
                private void load(OsuColour colours)
                {
                    Add(new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Colour = colours.Blue,
                        Depth = 2,
                    });
                    Add(new Triangles
                    {
                        RelativeSizeAxes = Axes.Both,
                        ColourDark = colours.BlueDarker,
                        ColourLight = colours.Blue,
                        Depth = 1,
                    });
                }
            }

            public enum PlayState
            {
                Play,
                Pause
            }

            public class ProgressBar : OsuSliderBar<double>
            {
                public override string TooltipText => GetTimeFormat((int)CurrentNumber.Value / 1000);

                public Action<double> OnSeek;

                private readonly OsuSpriteText nowTimeSpriteText;
                private readonly OsuSpriteText totalTimeSpriteText;

                public double StartTime
                {
                    set => CurrentNumber.MinValue = value;
                }

                public double EndTime
                {
                    set
                    {
                        CurrentNumber.MaxValue = value;
                        totalTimeSpriteText.Text = GetTimeFormat(((int)CurrentNumber.MaxValue - (int)CurrentNumber.MinValue) / 1000);
                    }
                }

                public double CurrentTime
                {
                    get => CurrentNumber.Value;
                    set
                    {
                        CurrentNumber.Value = value;
                        nowTimeSpriteText.Text = GetTimeFormat(((int)CurrentNumber.Value - (int)CurrentNumber.MinValue) / 1000);
                    }
                }

                public ProgressBar()
                {
                    AddRange(new Drawable[]
                    {
                        nowTimeSpriteText = new OsuSpriteText
                        {
                            Name = "Now time",
                            Position = new Vector2(-10, -2),
                            Text = "--:--",
                            UseFullGlyphHeight = false,
                            Origin = Anchor.CentreRight,
                            Anchor = Anchor.CentreLeft,
                            Font = new FontUsage(size: 15),
                            Alpha = 1
                        },
                        totalTimeSpriteText = new OsuSpriteText
                        {
                            Name = "End time",
                            Position = new Vector2(35, -2),
                            Text = "--:--",
                            UseFullGlyphHeight = false,
                            Origin = Anchor.CentreRight,
                            Anchor = Anchor.CentreRight,
                            Font = new FontUsage(size: 15),
                            Alpha = 1
                        }
                    });

                    StartTime = 0;
                    EndTime = 600000;
                    KeyboardStep = 1000f;
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

            public class ToolTipButton : TriangleButton, IHasTooltip
            {
                public string TooltipText { get; set; }
            }

            public class TooltipSpriteText : OsuSpriteText, IHasTooltip
            {
                public string TooltipText { get; set; }

                public TooltipSpriteText()
                {
                    UseFullGlyphHeight = false;
                    Origin = Anchor.CentreLeft;
                    Anchor = Anchor.TopLeft;
                    Font = new FontUsage(size: 20);
                    Alpha = 1;
                }
            }
        }
    }
}
