// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditor
    {
        private Dictionary<Mode, ICaretPositionAlgorithm> caretMovingAlgorithmSet = new Dictionary<Mode, ICaretPositionAlgorithm>();

        private void createAlgorithmList()
        {
            var lyrics = BindableLyrics.ToArray();
            caretMovingAlgorithmSet = new Dictionary<Mode, ICaretPositionAlgorithm>
            {
                { Mode.EditMode, new CuttingCaretPositionAlgorithm(lyrics) },
                { Mode.TypingMode, new TypingCaretPositionAlgorithm(lyrics) },
                { Mode.RecordMode, new TimeTagCaretPositionAlgorithm(lyrics) { Mode = RecordingMovingCaretMode } },
                { Mode.TimeTagEditMode, new TimeTagIndexCaretPositionAlgorithm(lyrics) }
            };
        }

        protected object GetCaretPositionAlgorithm()
        {
            return caretMovingAlgorithmSet[Mode];
        }

        public bool MoveCaret(MovingCaretAction action)
        {
            if (Mode == Mode.ViewMode)
                return false;

            var currentPosition = BindableCaretPosition.Value;
            var algorithm = GetCaretPositionAlgorithm();
            ICaretPosition position;

            switch (action)
            {
                case MovingCaretAction.Up:
                    position = algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveUp", currentPosition);
                    break;

                case MovingCaretAction.Down:
                    position = algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveDown", currentPosition);
                    break;

                case MovingCaretAction.Left:
                    position = algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveLeft", currentPosition);
                    break;

                case MovingCaretAction.Right:
                    position = algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveRight", currentPosition);
                    break;

                case MovingCaretAction.First:
                    position = algorithm.CallMethod<ICaretPosition>("MoveToFirst");
                    break;

                case MovingCaretAction.Last:
                    position = algorithm.CallMethod<ICaretPosition>("MoveToLast");
                    break;

                default:
                    throw new InvalidOperationException(nameof(action));
            }

            if (position == null)
                return false;

            MoveCaretToTargetPosition(position);
            return true;
        }

        public bool MoveCaretToTargetPosition(ICaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretMovable(position))
                return false;

            BindableHoverCaretPosition.Value = generatePosition(Mode);
            BindableCaretPosition.Value = position;
            return true;
        }

        public bool MoveHoverCaretToTargetPosition(ICaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretMovable(position))
                return false;

            BindableHoverCaretPosition.Value = position;
            return true;
        }

        public void ClearHoverCaretPosition()
        {
            BindableHoverCaretPosition.Value = generatePosition(Mode);
        }

        public void ResetPosition(Mode mode)
        {
            BindableCaretPosition.Value = generatePosition(mode);
            BindableHoverCaretPosition.Value = generatePosition(mode);
        }

        public bool CaretMovable(ICaretPosition position)
        {
            var algorithm = GetCaretPositionAlgorithm();
            return algorithm.CallMethod<bool, ICaretPosition>("PositionMovable", position);
        }

        private ICaretPosition generatePosition(Mode mode)
        {
            switch (mode)
            {
                case Mode.EditMode:
                case Mode.TypingMode:
                    return new TextCaretPosition(null, 0);

                case Mode.RecordMode:
                    return new TimeTagCaretPosition(null, null);

                case Mode.TimeTagEditMode:
                    return new TimeTagIndexCaretPosition(null, new TextIndex(0));

                default:
                    throw new ArgumentOutOfRangeException(nameof(Mode));
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
