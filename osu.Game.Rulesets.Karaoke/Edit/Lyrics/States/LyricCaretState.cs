// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class LyricCaretState : Component
    {
        public Bindable<ICaretPosition> BindableHoverCaretPosition { get; } = new Bindable<ICaretPosition>();

        public Bindable<ICaretPosition> BindableCaretPosition { get; } = new Bindable<ICaretPosition>();

        private ICaretPositionAlgorithm algorithm;

        public void ChangePositionAlgorithm(Lyric[] lyrics, LyricEditorMode lyricEditorMode, RecordingMovingCaretMode recordingMovingCaretMode)
        {
            algorithm = getAlgorithmByMode(lyricEditorMode);

            ICaretPositionAlgorithm getAlgorithmByMode(LyricEditorMode mode)
            {
                switch (mode)
                {
                    case LyricEditorMode.Manage:
                        return new CuttingCaretPositionAlgorithm(lyrics);

                    case LyricEditorMode.Typing:
                        return new TypingCaretPositionAlgorithm(lyrics);

                    case LyricEditorMode.EditRuby:
                    case LyricEditorMode.EditRomaji:
                        return new NavigateCaretPositionAlgorithm(lyrics);

                    case LyricEditorMode.CreateTimeTag:
                        return new TimeTagIndexCaretPositionAlgorithm(lyrics);

                    case LyricEditorMode.RecordTimeTag:
                        return new TimeTagCaretPositionAlgorithm(lyrics) { Mode = recordingMovingCaretMode };

                    case LyricEditorMode.AdjustTimeTag:
                    case LyricEditorMode.CreateNote:
                    case LyricEditorMode.CreateNotePosition:
                    case LyricEditorMode.AdjustNote:
                    case LyricEditorMode.Layout:
                    case LyricEditorMode.Singer:
                        return new NavigateCaretPositionAlgorithm(lyrics);

                    default:
                        return null;
                }
            }
        }

        public bool MoveCaret(MovingCaretAction action)
        {
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

        public void ResetPosition(LyricEditorMode mode)
        {
            var lyric = BindableCaretPosition.Value?.Lyric;

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
            return algorithm?.CallMethod<bool, ICaretPosition>("PositionMovable", position) ?? false;
        }

        public bool CaretEnabled => algorithm != null;
    }
}
