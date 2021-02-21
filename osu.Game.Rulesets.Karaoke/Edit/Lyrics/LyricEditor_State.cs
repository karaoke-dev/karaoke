// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditor
    {
        private Dictionary<Mode, ICaretPositionAlgorithm> caretMovingAlgorithmSet = new Dictionary<Mode, ICaretPositionAlgorithm>();

        private ICaretPositionAlgorithm caretMovingAlgorithm => caretMovingAlgorithmSet[Mode];

        private void createAlgorithmList()
        {
            var lyrics = BindableLyrics.ToArray();
            caretMovingAlgorithmSet = new Dictionary<Mode, ICaretPositionAlgorithm>
            {
                { Mode.EditMode, new CuttingCaretPositionAlgorithm(lyrics) },
                { Mode.TypingMode, new TypingCaretPositionAlgorithm(lyrics) },
                { Mode.RecordMode, new RecordingCaretPositionAlgorithm(lyrics, RecordingMovingCursorMode) },
                { Mode.TimeTagEditMode, new GenericCaretPositionAlgorithm(lyrics) }
            };
        }

        public bool MoveCaret(MovingCursorAction action)
        {
            if (Mode == Mode.ViewMode)
                return false;

            var currentPosition = BindableCursorPosition.Value;
            CursorPosition? position;

            switch (action)
            {
                case MovingCursorAction.Up:
                    position = caretMovingAlgorithm.MoveUp(currentPosition);
                    break;

                case MovingCursorAction.Down:
                    position = caretMovingAlgorithm.MoveDown(currentPosition);
                    break;

                case MovingCursorAction.Left:
                    position = caretMovingAlgorithm.MoveLeft(currentPosition);
                    break;

                case MovingCursorAction.Right:
                    position = caretMovingAlgorithm.MoveRight(currentPosition);
                    break;

                case MovingCursorAction.First:
                    position = caretMovingAlgorithm.MoveToFirst();
                    break;

                case MovingCursorAction.Last:
                    position = caretMovingAlgorithm.MoveToLast();
                    break;

                default:
                    throw new InvalidOperationException(nameof(action));
            }

            if (position == null)
                return false;

            movePositionTo(position.Value);
            return true;
        }

        public bool MoveCaretToTargetPosition(CursorPosition position)
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

        public bool MoveHoverCaretToTargetPosition(CursorPosition position)
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

        public void ClearHoverCaretPosition()
        {
            BindableHoverCursorPosition.Value = new CursorPosition();
        }

        public bool CaretMovable(CursorPosition position)
        {
            return caretMovingAlgorithm.PositionMovable(position);
        }

        private bool movePositionTo(CursorPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretMovable(position))
                return false;

            BindableHoverCursorPosition.Value = new CursorPosition();
            BindableCursorPosition.Value = position;
            return true;
        }

        private bool moveHoverPositionTo(CursorPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretMovable(position))
                return false;

            BindableHoverCursorPosition.Value = position;
            return true;
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
