// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Algorithms;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

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
                { Mode.RubyRomajiMode, new NavigateCaretPositionAlgorithm(lyrics) },
                { Mode.EditNoteMode, new NavigateCaretPositionAlgorithm(lyrics) },
                { Mode.RecordMode, new TimeTagCaretPositionAlgorithm(lyrics) { Mode = RecordingMovingCaretMode } },
                { Mode.TimeTagEditMode, new TimeTagIndexCaretPositionAlgorithm(lyrics) },
                { Mode.Layout, new NavigateCaretPositionAlgorithm(lyrics) },
                { Mode.Singer, new NavigateCaretPositionAlgorithm(lyrics) },
            };
        }

        protected object GetCaretPositionAlgorithm()
        {
            return caretMovingAlgorithmSet.GetOrDefault(Mode);
        }

        public bool MoveCaret(MovingCaretAction action)
        {
            var algorithm = GetCaretPositionAlgorithm();
            if (algorithm == null)
                return false;

            var currentPosition = BindableCaretPosition.Value;
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

            if (!CaretPositionMovable(position))
                return false;

            BindableHoverCaretPosition.Value = null;
            BindableCaretPosition.Value = position;
            return true;
        }

        public bool MoveHoverCaretToTargetPosition(ICaretPosition position)
        {
            if (position.Lyric == null)
                return false;

            if (!CaretPositionMovable(position))
                return false;

            BindableHoverCaretPosition.Value = position;
            return true;
        }

        public void ClearHoverCaretPosition()
        {
            BindableHoverCaretPosition.Value = null;
        }

        public void ResetPosition(Mode mode)
        {
            var lyric = BindableCaretPosition.Value?.Lyric;
            var algorithm = GetCaretPositionAlgorithm();

            if (algorithm != null)
            {
                if (lyric != null)
                {
                    BindableCaretPosition.Value = algorithm.CallMethod<ICaretPosition, Lyric>("MoveToTarget", lyric);
                    BindableHoverCaretPosition.Value = algorithm.CallMethod<ICaretPosition, Lyric>("MoveToTarget", lyric);
                }
                else
                {
                    BindableCaretPosition.Value = algorithm.CallMethod<ICaretPosition>("MoveToFirst");
                    BindableHoverCaretPosition.Value = algorithm.CallMethod<ICaretPosition>("MoveToFirst");
                }
            }
            else
            {
                BindableCaretPosition.Value = null;
                BindableHoverCaretPosition.Value = null;
            }
        }

        public bool CaretPositionMovable(ICaretPosition position)
        {
            var algorithm = GetCaretPositionAlgorithm();
            return algorithm?.CallMethod<bool, ICaretPosition>("PositionMovable", position) ?? false;
        }

        public bool CaretEnabled => GetCaretPositionAlgorithm() != null;

        public void ClearSelectedTimeTags()
        {
            SelectedTimeTags.Clear();
        }

        public void ClearSelectedTextTags()
        {
            SelectedTextTags.Clear();
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
        /// Able to create/delete ruby/romaji.
        /// </summary>
        RubyRomajiMode,

        /// <summary>
        /// Able to create/delete/mode/split/combine note.
        /// </summary>
        EditNoteMode,

        /// <summary>
        /// Click white-space to set current time into time-tag.
        /// </summary>
        RecordMode,

        /// <summary>
        /// Enable to create/delete and reset time tag.
        /// </summary>
        TimeTagEditMode,

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
