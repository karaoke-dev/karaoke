// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Game.Skinning;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditor : Container, IKeyBindingHandler<KaraokeEditAction>
    {
        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Resolved(canBeNull: true)]
        private IFrameBasedClock framedClock { get; set; }

        [Cached]
        private LyricEditorStateManager stateManager;

        private KaraokeLyricEditorSkin skin;
        private DrawableLyricEditList container;

        public LyricEditor()
        {
            Add(stateManager = new LyricEditorStateManager());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin())
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new DrawableLyricEditList
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };

            container.Items.BindTo(stateManager.BindableLyrics);
            if (lyricManager != null)
                container.OnOrderChanged += lyricManager.ChangeLyricOrder;

            stateManager.MoveCursor(MovingCursorAction.First);

            stateManager.BindableMode.BindValueChanged(e =>
            {
                // display add new lyric only with edit mode.
                container.DisplayBottomDrawable = e.NewValue == Mode.EditMode;
            }, true);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (lyricManager == null)
                return false;

            if (stateManager.Mode != Mode.TypingMode)
                return false;

            var position = stateManager.BindableCursorPosition.Value;

            switch (e.Key)
            {
                case Key.BackSpace:
                    // delete single character.
                    var deletedSuccess = lyricManager.DeleteLyricText(position);
                    if (deletedSuccess)
                        stateManager.MoveCursor(MovingCursorAction.Left);
                    return deletedSuccess;

                default:
                    return false;
            }
        }

        public bool OnPressed(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var isMoving = HandleMovingEvent(action);
            if (isMoving)
                return true;

            switch (stateManager.Mode)
            {
                case Mode.ViewMode:
                    return false;

                case Mode.EditMode:
                    return false;

                case Mode.TypingMode:
                    // will handle in OnKeyDown
                    return false;

                case Mode.RecordMode:
                    return HandleSetTimeEvent(action);

                case Mode.TimeTagEditMode:
                    return HandleCreateOrDeleterTimeTagEvent(action);

                default:
                    throw new IndexOutOfRangeException(nameof(stateManager.Mode));
            }
        }

        public void OnReleased(KaraokeEditAction action)
        {
        }

        protected bool HandleMovingEvent(KaraokeEditAction action)
        {
            // moving cursor action
            switch (action)
            {
                case KaraokeEditAction.Up:
                    return stateManager.MoveCursor(MovingCursorAction.Up);

                case KaraokeEditAction.Down:
                    return stateManager.MoveCursor(MovingCursorAction.Down);

                case KaraokeEditAction.Left:
                    return stateManager.MoveCursor(MovingCursorAction.Left);

                case KaraokeEditAction.Right:
                    return stateManager.MoveCursor(MovingCursorAction.Right);

                case KaraokeEditAction.First:
                    return stateManager.MoveCursor(MovingCursorAction.First);

                case KaraokeEditAction.Last:
                    return stateManager.MoveCursor(MovingCursorAction.Last);

                default:
                    return false;
            }
        }

        protected bool HandleSetTimeEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var currentTimeTag = stateManager.BindableRecordCursorPosition.Value;

            switch (action)
            {
                case KaraokeEditAction.ClearTime:
                    return lyricManager.ClearTimeTagTime(currentTimeTag);

                case KaraokeEditAction.SetTime:
                    if (framedClock == null)
                        return false;

                    var currentTime = framedClock.CurrentTime;
                    var setTimeSuccess = lyricManager.SetTimeTagTime(currentTimeTag, currentTime);
                    if (setTimeSuccess)
                        stateManager.MoveCursor(MovingCursorAction.Right);
                    return setTimeSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var position = stateManager.BindableCursorPosition.Value;

            switch (action)
            {
                case KaraokeEditAction.Create:
                    return lyricManager.AddTimeTagByPosition(position);

                case KaraokeEditAction.Remove:
                    return lyricManager.RemoveTimeTagByPosition(position);

                default:
                    return false;
            }
        }

        public float FontSize
        {
            get => skin.FontSize;
            set => skin.FontSize = value;
        }

        public Mode Mode
        {
            get => stateManager.Mode;
            set => stateManager.SetMode(value);
        }

        public LyricFastEditMode LyricFastEditMode
        {
            get => stateManager.FastEditMode;
            set => stateManager.SetFastEditMode(value);
        }

        public RecordingMovingCursorMode RecordingMovingCursorMode
        {
            get => stateManager.RecordingMovingCursorMode;
            set => stateManager.SetRecordingMovingCursorMode(value);
        }
    }
}
