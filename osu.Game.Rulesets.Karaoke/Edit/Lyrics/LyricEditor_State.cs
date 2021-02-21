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
                { Mode.RecordMode, new RecordingCaretPositionAlgorithm(lyrics, RecordingMovingCaretMode) },
                { Mode.TimeTagEditMode, new GenericCaretPositionAlgorithm(lyrics) }
            };
        }

        public bool MoveCaret(MovingCaretAction action)
        {
            if (Mode == Mode.ViewMode)
                return false;

            var currentPosition = BindableCaretPosition.Value;
            ICaretPosition? position;

            switch (action)
            {
                case MovingCaretAction.Up:
                    position = caretMovingAlgorithm.MoveUp(currentPosition);
                    break;

                case MovingCaretAction.Down:
                    position = caretMovingAlgorithm.MoveDown(currentPosition);
                    break;

                case MovingCaretAction.Left:
                    position = caretMovingAlgorithm.MoveLeft(currentPosition);
                    break;

                case MovingCaretAction.Right:
                    position = caretMovingAlgorithm.MoveRight(currentPosition);
                    break;

                case MovingCaretAction.First:
                    position = caretMovingAlgorithm.MoveToFirst();
                    break;

                case MovingCaretAction.Last:
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

        public bool MoveCaretToTargetPosition(ICaretPosition position)
        {
            switch (position.Mode)
            {
                case CaretMode.Edit:
                case CaretMode.Recording:
                    return movePositionTo(position);

                default:
                    throw new IndexOutOfRangeException(nameof(position.Mode));
            }
        }

        public bool MoveHoverCaretToTargetPosition(ICaretPosition position)
        {
            switch (position.Mode)
            {
                case CaretMode.Edit:
                case CaretMode.Recording:
                    return moveHoverPositionTo(position);

                default:
                    throw new IndexOutOfRangeException(nameof(position.Mode));
            }
        }

        public void ClearHoverCaretPosition()
        {
            BindableHoverCaretPosition.Value = new ICaretPosition();
        }

        public bool CaretMovable(ICaretPosition position)
        {
            return caretMovingAlgorithm.PositionMovable(position);
        }

        private bool movePositionTo(ICaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretMovable(position))
                return false;

            BindableHoverCaretPosition.Value = new ICaretPosition();
            BindableCaretPosition.Value = position;
            return true;
        }

        private bool moveHoverPositionTo(ICaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretMovable(position))
                return false;

            BindableHoverCaretPosition.Value = position;
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

    public enum RecordingMovingCaretMode
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

    public enum MovingCaretAction
    {
        Up,

        Down,

        Left,

        Right,

        First,

        Last,
    }
}
