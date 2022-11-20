// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose
{
    public abstract class Panel : FocusedOverlayContainer
    {
        private Sample? samplePopIn;
        private Sample? samplePopOut;

        private const float transition_length = 600;

        protected virtual string PopInSampleName => "UI/overlay-pop-in";
        protected virtual string PopOutSampleName => "UI/overlay-pop-out";

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        private readonly Box background;
        private readonly FillFlowContainer fillFlowContainer;

        protected Panel()
        {
            RelativeSizeAxes = Axes.Y;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Name = "Background",
                    RelativeSizeAxes = Axes.Both,
                },
                new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = fillFlowContainer = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(10),
                    },
                }
            };
        }

        protected abstract IReadOnlyList<Drawable> CreateSections();

        [BackgroundDependencyLoader(true)]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider, AudioManager audio)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableMode.BindValueChanged(x =>
            {
                background.Colour = colourProvider.Background2(state.Mode);
            }, true);

            samplePopIn = audio.Samples.Get(PopInSampleName);
            samplePopOut = audio.Samples.Get(PopOutSampleName);
        }

        private PanelDirection direction;

        public PanelDirection Direction
        {
            get => direction;
            set
            {
                if (direction == value)
                    return;

                direction = value;

                switch (direction)
                {
                    case PanelDirection.Left:
                        Anchor = Anchor.CentreLeft;
                        Origin = Anchor.CentreLeft;
                        break;

                    case PanelDirection.Right:
                        Anchor = Anchor.CentreRight;
                        Origin = Anchor.CentreRight;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }

                if (State.Value == Visibility.Hidden)
                {
                    X = getHideXPosition();
                }
            }
        }

        protected override void PopIn()
        {
            base.PopIn();
            samplePopIn?.Play();

            // todo: adjust the effect.
            this.MoveToX(0, transition_length, Easing.OutQuint);
            this.FadeTo(1, transition_length, Easing.OutQuint);

            // should load the content after opened.
            fillFlowContainer.Children = CreateSections();
        }

        protected override void PopOut()
        {
            base.PopOut();
            samplePopOut?.Play();

            float width = getHideXPosition();
            this.MoveToX(width, transition_length, Easing.OutQuint);
            this.FadeTo(0, transition_length, Easing.OutQuint).OnComplete(_ =>
            {
                // should clear the content if close.
                fillFlowContainer.Clear();
            });
        }

        private float getHideXPosition() =>
            direction switch
            {
                PanelDirection.Left => -DrawWidth,
                PanelDirection.Right => DrawWidth,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
