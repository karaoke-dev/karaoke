// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditor
    {
        public Bindable<Mode> BindableMode { get; } = new Bindable<Mode>();

        public Bindable<LyricFastEditMode> BindableFastEditMode { get; } = new Bindable<LyricFastEditMode>();

        public Bindable<RecordingMovingCursorMode> BindableRecordingMovingCursorMode { get; } = new Bindable<RecordingMovingCursorMode>();

        public BindableBool BindableAutoFocusEditLyric { get; } = new BindableBool();

        public BindableInt BindableAutoFocusEditLyricSkipRows { get; } = new BindableInt();

        public BindableList<Lyric> BindableLyrics { get; } = new BindableList<Lyric>();

        public Bindable<CursorPosition> BindableHoverCursorPosition { get; } = new Bindable<CursorPosition>();

        public Bindable<CursorPosition> BindableCursorPosition { get; } = new Bindable<CursorPosition>();

        private Dictionary<Mode, ICursorPositionAlgorithm> cursorMovingAlgorithmSet = new Dictionary<Mode, ICursorPositionAlgorithm>();

        private ICursorPositionAlgorithm algorithm => cursorMovingAlgorithmSet[Mode];

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
            createAlgorithmList();

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

        private void createAlgorithmList()
        {
            cursorMovingAlgorithmSet = new Dictionary<Mode, ICursorPositionAlgorithm>
            {
                { Mode.TypingMode, new TypingCursorPositionAlgorithm(BindableLyrics.ToArray())},
                { Mode.RecordMode, new RecordingCursorPositionAlgorithm(BindableLyrics.ToArray(), RecordingMovingCursorMode)},
                { Mode.TimeTagEditMode, new GenericCursorPositionAlgorithm(BindableLyrics.ToArray())}
            };
        }

        public bool MoveCursor(MovingCursorAction action)
        {
            switch (Mode)
            {
                case Mode.ViewMode:
                    return false;

                case Mode.EditMode:
                case Mode.TypingMode:
                case Mode.RecordMode:
                case Mode.TimeTagEditMode:
                    return moveCursor(action);

                default:
                    throw new IndexOutOfRangeException(nameof(Mode));
            }
        }

        private bool moveCursor(MovingCursorAction action)
        {
            var currentPosition = BindableCursorPosition.Value;
            CursorPosition? position;

            switch (action)
            {
                case MovingCursorAction.Up:
                    position = algorithm.MoveUp(currentPosition);
                    break;

                case MovingCursorAction.Down:
                    position = algorithm.MoveDown(currentPosition);
                    break;

                case MovingCursorAction.Left:
                    position = algorithm.MoveLeft(currentPosition);
                    break;

                case MovingCursorAction.Right:
                    position = algorithm.MoveRight(currentPosition);
                    break;

                case MovingCursorAction.First:
                    position = algorithm.MoveToFirst();
                    break;

                case MovingCursorAction.Last:
                    position = algorithm.MoveToLast();
                    break;

                default:
                    throw new InvalidOperationException(nameof(action));
            }

            if (position == null)
                return false;

            movePositionTo(position.Value);
            return true;
        }

        public bool MoveCursorToTargetPosition(CursorPosition position)
        {
            switch (position.Mode)
            {
                case CursorMode.Edit:
                case CursorMode.Recording:
                    return movePositionTo(position);
                default:
                    throw new IndexOutOfRangeException(nameof(position.Mode));
            }
        }

        public bool MoveHoverCursorToTargetPosition(CursorPosition position)
        {
            switch (position.Mode)
            {
                case CursorMode.Edit:
                case CursorMode.Recording:
                        return moveHoverPositionTo(position);
                default:
                    throw new IndexOutOfRangeException(nameof(position.Mode));
            }
        }

        private bool movePositionTo(CursorPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CursorMovable(position))
                return false;

            BindableHoverCursorPosition.Value = new CursorPosition();
            BindableCursorPosition.Value = position;
            return true;
        }

        private bool moveHoverPositionTo(CursorPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CursorMovable(position))
                return false;

            BindableHoverCursorPosition.Value = position;
            return true;
        }

        public void ClearHoverCursorPosition()
        {
            BindableHoverCursorPosition.Value = new CursorPosition();
        }

        public bool CursorMovable(CursorPosition position)
        {
            return algorithm.PositionMovable(position);
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
