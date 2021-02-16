// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager : Component
    {
        public Bindable<Mode> BindableMode { get; } = new Bindable<Mode>();

        public Bindable<LyricFastEditMode> BindableFastEditMode { get; } = new Bindable<LyricFastEditMode>();

        public Bindable<RecordingMovingCursorMode> BindableRecordingMovingCursorMode { get; } = new Bindable<RecordingMovingCursorMode>();

        public BindableBool BindableAutoFocusEditLyric { get; } = new BindableBool();

        public BindableInt BindableAutoFocusEditLyricSkipRows { get; } = new BindableInt();

        public Mode Mode => BindableMode.Value;

        public LyricFastEditMode FastEditMode => BindableFastEditMode.Value;

        public RecordingMovingCursorMode RecordingMovingCursorMode => BindableRecordingMovingCursorMode.Value;

        public BindableList<Lyric> BindableLyrics { get; } = new BindableList<Lyric>();

        // Lyrics is not lock and can be accessible.
        protected IEnumerable<Lyric> Lyrics => LyricsUtils.FindUnlockLyrics(OrderUtils.Sorted(BindableLyrics));

        public Bindable<CursorPosition> BindableHoverCursorPosition { get; } = new Bindable<CursorPosition>();

        public Bindable<CursorPosition> BindableCursorPosition { get; } = new Bindable<CursorPosition>();

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
        }

        public void SetMode(Mode mode)
        {
            BindableMode.Value = mode;

            switch (mode)
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

        public void SetFastEditMode(LyricFastEditMode fastEditMode)
        {
            BindableFastEditMode.Value = fastEditMode;
        }

        public void SetRecordingMovingCursorMode(RecordingMovingCursorMode mode)
        {
            BindableRecordingMovingCursorMode.Value = mode;

            // todo : might move cursor to valid position.
        }

        public void SetBindableAutoFocusEditLyric(bool focus)
        {
            BindableAutoFocusEditLyric.Value = focus;
        }

        public void SetBindableAutoFocusEditLyricSkipRows(int row)
        {
            BindableAutoFocusEditLyricSkipRows.Value = row;
        }

        public bool MoveCursor(MovingCursorAction action)
        {
            switch (Mode)
            {
                case Mode.ViewMode:
                    return false;

                case Mode.EditMode:
                case Mode.TypingMode:
                    return moveCursor(action);

                case Mode.RecordMode:
                    return moveRecordCursor(action);

                case Mode.TimeTagEditMode:
                    return moveCursor(action);

                default:
                    throw new IndexOutOfRangeException(nameof(Mode));
            }
        }
    }

    public enum Mode
    {
        /// <summary>
        /// Cannot edit anything except each lyric's left-side part.
        /// </summary>
        ViewMode,

        /// <summary>
        /// Can create/delete/mode/split/combine lyric.
        /// </summary>
        EditMode,

        /// <summary>
        /// Able to typing lyric.
        /// </summary>
        TypingMode,

        /// <summary>
        /// Click white-space to set current time into time-tag.
        /// </summary>
        RecordMode,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        TimeTagEditMode
    }

    public enum LyricFastEditMode
    {
        /// <summary>
        /// User can only see start and end time.
        /// </summary>
        None,

        /// <summary>
        /// Can edit each lyric's layout.
        /// </summary>
        Layout,

        /// <summary>
        /// Can edit each lyric's singer.
        /// </summary>
        Singer,

        /// <summary>
        /// Can edit each lyric's language.
        /// </summary>
        Language,

        /// <summary>
        /// Display lyric time-tag's first and last time.
        /// </summary>
        TimeTag,
    }

    public enum RecordingMovingCursorMode
    {
        /// <summary>
        /// Move to any tag
        /// </summary>
        None,

        /// <summary>
        /// Only move to next start tag.
        /// </summary>
        OnlyStartTag,

        /// <summary>
        /// Only move to next end tag.
        /// </summary>
        OnlyEndTag,
    }

    public enum MovingCursorAction
    {
        Up,

        Down,

        Left,

        Right,

        First,

        Last,
    }
}
