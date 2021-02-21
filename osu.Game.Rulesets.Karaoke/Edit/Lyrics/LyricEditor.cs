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

        [Resolved]
        private EditorClock editorClock { get; set; }

        public Bindable<Mode> BindableMode { get; } = new Bindable<Mode>();

        public Bindable<LyricFastEditMode> BindableFastEditMode { get; } = new Bindable<LyricFastEditMode>();

        public Bindable<RecordingMovingCaretMode> BindableRecordingMovingCaretMode { get; } = new Bindable<RecordingMovingCaretMode>();

        public BindableBool BindableAutoFocusEditLyric { get; } = new BindableBool();

        public BindableInt BindableAutoFocusEditLyricSkipRows { get; } = new BindableInt();

        public BindableList<Lyric> BindableLyrics { get; } = new BindableList<Lyric>();

        public Bindable<CaretPosition> BindableHoverCaretPosition { get; } = new Bindable<CaretPosition>();

        public Bindable<CaretPosition> BindableCaretPosition { get; } = new Bindable<CaretPosition>();

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

            MoveCaret(MovingCaretAction.First);

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
                {
                    BindableLyrics.Add(lyric);
                    createAlgorithmList();
                }
            };
            beatmap.HitObjectRemoved += e =>
            {
                if (e is Lyric lyric)
                {
                    BindableLyrics.Remove(lyric);
                    createAlgorithmList();
                }
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

            var position = BindableCaretPosition.Value;

            switch (e.Key)
            {
                case Key.BackSpace:
                    // delete single character.
                    var deletedSuccess = lyricManager.DeleteLyricText(position);
                    if (deletedSuccess)
                        MoveCaret(MovingCaretAction.Left);
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
            // moving caret action
            switch (action)
            {
                case KaraokeEditAction.Up:
                    return MoveCaret(MovingCaretAction.Up);

                case KaraokeEditAction.Down:
                    return MoveCaret(MovingCaretAction.Down);

                case KaraokeEditAction.Left:
                    return MoveCaret(MovingCaretAction.Left);

                case KaraokeEditAction.Right:
                    return MoveCaret(MovingCaretAction.Right);

                case KaraokeEditAction.First:
                    return MoveCaret(MovingCaretAction.First);

                case KaraokeEditAction.Last:
                    return MoveCaret(MovingCaretAction.Last);

                default:
                    return false;
            }
        }

        protected bool HandleSetTimeEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var caretPosition = BindableCaretPosition.Value;
            if (caretPosition.Mode != CaretMode.Recording)
                return false;

            var currentTimeTag = caretPosition.TimeTag;

            switch (action)
            {
                case KaraokeEditAction.ClearTime:
                    return lyricManager.ClearTimeTagTime(currentTimeTag);

                case KaraokeEditAction.SetTime:
                    var currentTime = editorClock.CurrentTime;
                    var setTimeSuccess = lyricManager.SetTimeTagTime(currentTimeTag, currentTime);
                    if (setTimeSuccess)
                        MoveCaret(MovingCaretAction.Right);
                    return setTimeSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var position = BindableCaretPosition.Value;

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
                        MoveCaret(MovingCaretAction.First);
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

        public RecordingMovingCaretMode RecordingMovingCaretMode
        {
            get => BindableRecordingMovingCaretMode.Value;
            set
            {
                if (BindableRecordingMovingCaretMode.Value == value)
                    return;

                BindableRecordingMovingCaretMode.Value = value;
                createAlgorithmList();

                // todo : might move caret to valid position.
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
