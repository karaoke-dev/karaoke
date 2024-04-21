// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class RecordingTapControl : CompositeDrawable, IKeyBindingHandler<KaraokeEditAction>
{
    [Resolved]
    private KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    [Resolved]
    private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private EditorClock editorClock { get; set; } = null!;

    private InlineButton undoButton = null!;
    private InlineButton resetButton = null!;
    private TapButton tapButton = null!;

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChildren = new Drawable[]
        {
            new Container
            {
                RelativeSizeAxes = Axes.Y,
                Anchor = Anchor.Centre,
                Origin = Anchor.CentreRight,
                Height = 0.98f,
                Width = TapButton.SIZE / 1.3f,
                Masking = true,
                CornerRadius = 15,
                Children = new Drawable[]
                {
                    undoButton = new InlineButton(FontAwesome.Solid.Trash, Anchor.TopLeft)
                    {
                        BackgroundColour = colourProvider.Background1(state.Mode),
                        RelativeSizeAxes = Axes.Both,
                        Height = 0.49f,
                        Action = reset,
                    },
                    resetButton = new InlineButton(FontAwesome.Solid.AngleLeft, Anchor.BottomLeft)
                    {
                        BackgroundColour = colourProvider.Background1(state.Mode),
                        RelativeSizeAxes = Axes.Both,
                        Height = 0.49f,
                        Anchor = Anchor.BottomLeft,
                        Origin = Anchor.BottomLeft,
                        Action = undo,
                    },
                },
            },
            tapButton = new TapButton
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Tapped = onTapped,
            },
        };
    }

    public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
    {
        switch (e.Action)
        {
            case KaraokeEditAction.ClearTime:
                resetButton.TriggerClick();

                return true;

            case KaraokeEditAction.SetTime:
                tapButton.TriggerClick();

                return true;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
    {
    }

    private void reset()
    {
        if (lyricCaretState.CaretPosition is not RecordingTimeTagCaretPosition timeTagCaretPosition)
            throw new InvalidOperationException();

        var timingInfo = timeTagCaretPosition.Lyric.LyricTimingInfo;

        lyricTimeTagsChangeHandler.ClearAllTimeTagTime();

        if (timingInfo != null)
        {
            editorClock.Seek(timingInfo.StartTime - 1000);
        }

        if (lyricCaretState.GetCaretPositionByAction(MovingCaretAction.FirstIndex)?.Lyric != timeTagCaretPosition.Lyric)
            return;

        lyricCaretState.MoveCaret(MovingCaretAction.FirstIndex);
    }

    private void undo()
    {
        if (lyricCaretState.CaretPosition is not RecordingTimeTagCaretPosition timeTagCaretPosition)
            throw new InvalidOperationException();

        double? currentTimeTagTime = timeTagCaretPosition.TimeTag.Time;

        var timeTag = timeTagCaretPosition.TimeTag;
        lyricTimeTagsChangeHandler.ClearTimeTagTime(timeTag);

        if (lyricCaretState.GetCaretPositionByAction(MovingCaretAction.PreviousIndex)?.Lyric != timeTagCaretPosition.Lyric)
            return;

        lyricCaretState.MoveCaret(MovingCaretAction.PreviousIndex);

        if (currentTimeTagTime != null)
        {
            editorClock.Seek(currentTimeTagTime.Value - 1000);
        }
        else
        {
            editorClock.Seek(editorClock.CurrentTime - 1000);
        }
    }

    private void onTapped(double currentTime)
    {
        if (lyricCaretState.CaretPosition is not RecordingTimeTagCaretPosition timeTagCaretPosition)
            throw new InvalidOperationException();

        var timeTag = timeTagCaretPosition.TimeTag;
        lyricTimeTagsChangeHandler.SetTimeTagTime(timeTag, currentTime);

        if (lyricEditorConfigManager.Get<bool>(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNextTimeTag))
            lyricCaretState.MoveCaret(MovingCaretAction.NextIndex);
    }

    private partial class InlineButton : OsuButton
    {
        private readonly IconUsage icon;
        private readonly Anchor anchor;

        private SpriteIcon spriteIcon = null!;

        [Resolved]
        private ILyricEditorState lyricEditorState { get; set; } = null!;

        [Resolved]
        private LyricEditorColourProvider colourProvider { get; set; } = null!;

        public InlineButton(IconUsage icon, Anchor anchor)
        {
            this.icon = icon;
            this.anchor = anchor;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Content.CornerRadius = 0;
            Content.Masking = false;

            BackgroundColour = colourProvider.Background2(lyricEditorState.Mode);

            Content.Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(15),
                Children = new Drawable[]
                {
                    spriteIcon = new SpriteIcon
                    {
                        Icon = icon,
                        Size = new Vector2(22),
                        Anchor = anchor,
                        Origin = anchor,
                        Colour = colourProvider.Background1(lyricEditorState.Mode),
                    },
                },
            });
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            // scale looks bad so don't call base.
            return false;
        }

        protected override bool OnHover(HoverEvent e)
        {
            spriteIcon.FadeColour(colourProvider.Content2(lyricEditorState.Mode), 200, Easing.OutQuint);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            spriteIcon.FadeColour(colourProvider.Background1(lyricEditorState.Mode), 200, Easing.OutQuint);
            base.OnHoverLost(e);
        }
    }
}
