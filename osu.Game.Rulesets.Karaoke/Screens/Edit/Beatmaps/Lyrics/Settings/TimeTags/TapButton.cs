// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Threading;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Screens.Edit;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

internal partial class TapButton : CircularContainer
{
    public const float SIZE = 140;

    public Action<double>? Tapped;

    [Resolved]
    private EditorClock editorClock { get; set; } = null!;

    [Resolved]
    private ILyricEditorState lyricEditorState { get; set; } = null!;

    [Resolved]
    private LyricEditorColourProvider colourProvider { get; set; } = null!;

    private Circle hoverLayer = null!;

    private CircularContainer innerCircle = null!;
    private Box innerCircleHighlight = null!;

    private int currentIndex = 0;

    private Container scaleContainer = null!;
    private Container<Light> lights = null!;
    private Container lightsGlow = null!;
    private OsuSpriteText timeTagInfoText = null!;
    private Container textContainer = null!;

    private bool grabbedMouseDown;

    private ScheduledDelegate? resetDelegate;

    private const double transition_length = 500;

    private const float angular_light_gap = 0.007f;

    private readonly IBindable<ICaretPosition?> bindableCaret = new Bindable<ICaretPosition?>();

    [BackgroundDependencyLoader]
    private void load(ILyricCaretState lyricCaretState)
    {
        bindableCaret.BindTo(lyricCaretState.BindableCaretPosition);

        Size = new Vector2(SIZE);

        const float ring_width = 10;
        const float light_padding = 3;

        InternalChild = scaleContainer = new Container
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colourProvider.Background4(lyricEditorState.Mode),
                },
                lights = new Container<Light>
                {
                    RelativeSizeAxes = Axes.Both,
                },
                new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "outer masking",
                    Masking = true,
                    BorderThickness = light_padding,
                    BorderColour = colourProvider.Background4(lyricEditorState.Mode),
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Color4.Black,
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                            AlwaysPresent = true,
                        },
                    },
                },
                new Circle
                {
                    Name = "inner masking",
                    Size = new Vector2(SIZE - ring_width * 2 + light_padding * 2),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = colourProvider.Background4(lyricEditorState.Mode),
                },
                lightsGlow = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
                innerCircle = new CircularContainer
                {
                    Size = new Vector2(SIZE - ring_width * 2),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = colourProvider.Background2(lyricEditorState.Mode),
                            RelativeSizeAxes = Axes.Both,
                        },
                        innerCircleHighlight = new Box
                        {
                            Colour = colourProvider.Colour3(lyricEditorState.Mode),
                            Blending = BlendingParameters.Additive,
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                        },
                        textContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background1(lyricEditorState.Mode),
                            Children = new Drawable[]
                            {
                                new OsuSpriteText
                                {
                                    Font = OsuFont.Torus.With(size: 34, weight: FontWeight.SemiBold),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.BottomCentre,
                                    Y = 5,
                                    Text = "Tap",
                                },
                                timeTagInfoText = new OsuSpriteText
                                {
                                    Font = OsuFont.Torus.With(size: 18, weight: FontWeight.Regular),
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.TopCentre,
                                    Y = 2,
                                },
                            },
                        },
                        hoverLayer = new Circle
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background1(lyricEditorState.Mode).Opacity(0.3f),
                            Blending = BlendingParameters.Additive,
                            Alpha = 0,
                        },
                    },
                },
            },
        };

        reset();

        bindableCaret.BindValueChanged(x =>
        {
            if (x.NewValue is RecordingTimeTagCaretPosition newCaret)
            {
                updateTimeTagAmount(newCaret.GetTotalTimeTags());
                updateCurrentTimeTag(newCaret.GetCurrentTimeTagIndex());
            }
            else
            {
                updateTimeTagAmount(0);
            }
        }, true);
    }

    private void updateTimeTagAmount(int amount)
    {
        if (lights.Children.Count == amount)
            return;

        lights.Clear();
        lightsGlow.Clear();

        for (int i = 0; i < amount; i++)
        {
            var light = new Light(amount)
            {
                Rotation = i * (360f / amount) + 360 * angular_light_gap / 2,
            };

            lights.Add(light);
            lightsGlow.Add(light.Glow.CreateProxy());
        }
    }

    private void updateCurrentTimeTag(int currentTimeTagIndex)
    {
        currentIndex = currentTimeTagIndex;

        for (int i = 0; i < lights.Children.Count; i++)
        {
            bool isTapped = i <= currentIndex;
            lights.Children[i].IsTapped = isTapped;
        }
    }

    public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) =>
        hoverLayer.ReceivePositionalInputAt(screenSpacePos);

    private ColourInfo textColour
    {
        get
        {
            if (grabbedMouseDown)
                return colourProvider.Background4(lyricEditorState.Mode);

            if (IsHovered)
                return colourProvider.Content2(lyricEditorState.Mode);

            return colourProvider.Background1(lyricEditorState.Mode);
        }
    }

    protected override bool OnHover(HoverEvent e)
    {
        hoverLayer.FadeIn(transition_length, Easing.OutQuint);
        textContainer.FadeColour(textColour, transition_length, Easing.OutQuint);
        return true;
    }

    protected override void OnHoverLost(HoverLostEvent e)
    {
        hoverLayer.FadeOut(transition_length, Easing.OutQuint);
        textContainer.FadeColour(textColour, transition_length, Easing.OutQuint);
        base.OnHoverLost(e);
    }

    protected override bool OnClick(ClickEvent e)
    {
        Tapped?.Invoke(editorClock.CurrentTime);

        mouseDownAnimation();
        mouseUpAnimation();

        return true;
    }

    private void mouseDownAnimation()
    {
        const double in_duration = 100;

        grabbedMouseDown = true;

        resetDelegate?.Cancel();

        textContainer.FadeColour(textColour, in_duration, Easing.OutQuint);

        scaleContainer.ScaleTo(0.99f, in_duration, Easing.OutQuint);
        innerCircle.ScaleTo(0.96f, in_duration, Easing.OutQuint);

        innerCircleHighlight
            .FadeIn(50, Easing.OutQuint)
            .FlashColour(Color4.White, 1000, Easing.OutQuint);

        lights.ForEach(x => x.Hide());
        lights[currentIndex].Show();
    }

    private void mouseUpAnimation()
    {
        const double out_duration = 800;

        grabbedMouseDown = false;

        textContainer.FadeColour(textColour, out_duration, Easing.OutQuint);

        scaleContainer.ScaleTo(1, out_duration, Easing.OutQuint);
        innerCircle.ScaleTo(1, out_duration, Easing.OutQuint);

        innerCircleHighlight.FadeOut(out_duration, Easing.OutQuint);

        resetDelegate = Scheduler.AddDelayed(reset, 1000);
    }

    private void reset()
    {
        timeTagInfoText.FadeOut(transition_length, Easing.OutQuint);
        timeTagInfoText.FadeIn(800, Easing.OutQuint);

        foreach (var light in lights)
            light.Hide();
    }

    protected override void Update()
    {
        base.Update();

        timeTagInfoText.Text = editorClock.CurrentTime.ToEditorFormattedString();
    }

    // todo: light should have states:
    // 1. pending
    // 2. finished
    private partial class Light : CompositeDrawable
    {
        public Drawable Glow { get; private set; } = null!;

        private CircularProgress circularProgress = null!;
        private Container fillContent = null!;

        [Resolved]
        private ILyricEditorState lyricEditorState { get; set; } = null!;

        [Resolved]
        private LyricEditorColourProvider colourProvider { get; set; } = null!;

        private readonly int lightAmount;

        public Light(int lightAmount)
        {
            this.lightAmount = lightAmount;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Size = new Vector2(0.98f); // Avoid bleed into masking edge.

            InternalChildren = new Drawable[]
            {
                circularProgress = new CircularProgress
                {
                    RelativeSizeAxes = Axes.Both,
                    Progress = 1f / lightAmount - angular_light_gap,
                },
                fillContent = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    Colour = colourProvider.Colour1(lyricEditorState.Mode),
                    Children = new[]
                    {
                        new CircularProgress
                        {
                            RelativeSizeAxes = Axes.Both,
                            Progress = 1f / lightAmount - angular_light_gap,
                            Blending = BlendingParameters.Additive,
                        },
                        // Please do not try and make sense of this.
                        // Getting the visual effect I was going for relies on what I can only imagine is broken implementation
                        // of `PadExtent`. If that's ever fixed in the future this will likely need to be adjusted.
                        Glow = new CircularProgress
                        {
                            RelativeSizeAxes = Axes.Both,
                            Progress = 1f / lightAmount - 0.01f,
                            Blending = BlendingParameters.Additive,
                        }.WithEffect(new GlowEffect
                        {
                            Colour = colourProvider.Colour1(lyricEditorState.Mode).Opacity(0.4f),
                            BlurSigma = new Vector2(9f),
                            Strength = 10,
                            PadExtent = true,
                        }),
                    },
                },
            };

            updateColour();
        }

        private bool isTapped = false;

        public bool IsTapped
        {
            get => isTapped;
            set
            {
                if (value == isTapped)
                    return;

                isTapped = value;

                updateColour();
            }
        }

        private void updateColour()
        {
            circularProgress.Colour = isTapped ? colourProvider.Colour1(lyricEditorState.Mode) : colourProvider.Background2(lyricEditorState.Mode);
        }

        public override void Show()
        {
            fillContent
                .FadeIn(50, Easing.OutQuint)
                .FlashColour(Color4.White, 1000, Easing.OutQuint);
        }

        public override void Hide()
        {
            fillContent
                .FadeOut(300, Easing.OutQuint);
        }
    }
}
