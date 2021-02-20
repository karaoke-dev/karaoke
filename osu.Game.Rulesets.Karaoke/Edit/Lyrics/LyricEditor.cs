// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    [Cached(typeof(ILyricEditorState))]
    public partial class LyricEditor : Container, ILyricEditorState, IKeyBindingHandler<KaraokeEditAction>
    {
        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Resolved(canBeNull: true)]
        private IFrameBasedClock framedClock { get; set; }

        public Bindable<Mode> BindableMode { get; } = new Bindable<Mode>();

        public Bindable<LyricFastEditMode> BindableFastEditMode { get; } = new Bindable<LyricFastEditMode>();

        public Bindable<RecordingMovingCursorMode> BindableRecordingMovingCursorMode { get; } = new Bindable<RecordingMovingCursorMode>();

        public BindableBool BindableAutoFocusEditLyric { get; } = new BindableBool();

        public BindableInt BindableAutoFocusEditLyricSkipRows { get; } = new BindableInt();

        public BindableList<Lyric> BindableLyrics { get; } = new BindableList<Lyric>();

        public Bindable<CursorPosition> BindableHoverCursorPosition { get; } = new Bindable<CursorPosition>();

        public Bindable<CursorPosition> BindableCursorPosition { get; } = new Bindable<CursorPosition>();

        private readonly KaraokeLyricEditorSkin skin;
        private readonly DrawableLyricEditList container;

        public LyricEditor()
        {
            Child = new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin())
            {
                RelativeSizeAxes = Axes.Both,
                Child = container = new DrawableLyricEditList
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };

            container.Items.BindTo(BindableLyrics);
            if (lyricManager != null)
                container.OnOrderChanged += lyricManager.ChangeLyricOrder;

            MoveCursor(MovingCursorAction.First);

            BindableMode.BindValueChanged(e =>
            {
                // display add new lyric only with edit mode.
                container.DisplayBottomDrawable = e.NewValue == Mode.EditMode;
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            // load lyric in here
            var lyrics = OrderUtils.Sorted(beatmap.HitObjects.OfType<Lyric>());
            BindableLyrics.AddRange(lyrics);

            // need to check is there any lyric added or removed.
            beatmap.HitObjectAdded += e =>
            {
                if (e is Lyric lyric)
                    BindableLyrics.Add(lyric);
            };
            beatmap.HitObjectRemoved += e =>
            {
                if (e is Lyric lyric)
                    BindableLyrics.Remove(lyric);
            };

            // create algorithm set
            createAlgorithmList();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (lyricManager == null)
                return false;

            if (Mode != Mode.TypingMode)
                return false;

            var position = BindableCursorPosition.Value;

            switch (e.Key)
            {
                case Key.BackSpace:
                    // delete single character.
                    var deletedSuccess = lyricManager.DeleteLyricText(position);
                    if (deletedSuccess)
                        MoveCursor(MovingCursorAction.Left);
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

            switch (Mode)
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
                    throw new IndexOutOfRangeException(nameof(Mode));
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
                    return MoveCursor(MovingCursorAction.Up);

                case KaraokeEditAction.Down:
                    return MoveCursor(MovingCursorAction.Down);

                case KaraokeEditAction.Left:
                    return MoveCursor(MovingCursorAction.Left);

                case KaraokeEditAction.Right:
                    return MoveCursor(MovingCursorAction.Right);

                case KaraokeEditAction.First:
                    return MoveCursor(MovingCursorAction.First);

                case KaraokeEditAction.Last:
                    return MoveCursor(MovingCursorAction.Last);

                default:
                    return false;
            }
        }

        protected bool HandleSetTimeEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var cursorPosition = BindableCursorPosition.Value;
            if (cursorPosition.Mode != CursorMode.Recording)
                return false;

            var currentTimeTag = cursorPosition.TimeTag;

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
                        MoveCursor(MovingCursorAction.Right);
                    return setTimeSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var position = BindableCursorPosition.Value;

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
            get => BindableMode.Value;
            set
            {
                if (BindableMode.Value == value)
                    return;

                BindableMode.Value = value;

                switch (Mode)
                {
                    case Mode.ViewMode:
                    case Mode.EditMode:
                    case Mode.TypingMode:
                        return;

                    case Mode.RecordMode:
                        MoveCursor(MovingCursorAction.First);
                        return;

                    case Mode.TimeTagEditMode:
                        return;

                    default:
                        throw new IndexOutOfRangeException(nameof(Mode));
                }
            }
        }

        public LyricFastEditMode LyricFastEditMode
        {
            get => BindableFastEditMode.Value;
            set => BindableFastEditMode.Value = value;
        }

        public RecordingMovingCursorMode RecordingMovingCursorMode
        {
            get => BindableRecordingMovingCursorMode.Value;
            set
            {
                if (BindableRecordingMovingCursorMode.Value == value)
                    return;

                BindableRecordingMovingCursorMode.Value = value;
                createAlgorithmList();

                // todo : might move cursor to valid position.
            }
        }

        public bool AutoFocusEditLyric
        {
            get => BindableAutoFocusEditLyric.Value;
            set => BindableAutoFocusEditLyric.Value = value;
        }

        public int AutoFocusEditLyricSkipRows
        {
            get => BindableAutoFocusEditLyricSkipRows.Value;
            set => BindableAutoFocusEditLyricSkipRows.Value = value;
        }
    }
}
