// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.ComponentModel;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public class LyricCaretState
    {
        public Bindable<ICaretPosition> BindableHoverCaretPosition { get; } = new();

        public Bindable<ICaretPosition> BindableCaretPosition { get; } = new();

        private ICaretPositionAlgorithm algorithm;

        public void ChangePositionAlgorithm(Lyric[] lyrics, LyricEditorMode lyricEditorMode, MovingTimeTagCaretMode movingTimeTagCaretMode)
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
                        return new TimeTagIndexCaretPositionAlgorithm(lyrics) { Mode = movingTimeTagCaretMode };

                    case LyricEditorMode.RecordTimeTag:
                        return new TimeTagCaretPositionAlgorithm(lyrics) { Mode = movingTimeTagCaretMode };

                    case LyricEditorMode.AdjustTimeTag:
                    case LyricEditorMode.EditNote:
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

            var position = action switch
            {
                MovingCaretAction.Up => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveUp", currentPosition),
                MovingCaretAction.Down => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveDown", currentPosition),
                MovingCaretAction.Left => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveLeft", currentPosition),
                MovingCaretAction.Right => algorithm.CallMethod<ICaretPosition, ICaretPosition>("MoveRight", currentPosition),
                MovingCaretAction.First => algorithm.CallMethod<ICaretPosition>("MoveToFirst"),
                MovingCaretAction.Last => algorithm.CallMethod<ICaretPosition>("MoveToLast"),
                _ => throw new InvalidEnumArgumentException(nameof(action))
            };

            if (position == null)
                return false;

            MoveCaretToTargetPosition(position);
            return true;
        }

        public void MoveCaretToTargetPosition(Lyric lyric)
        {
            if (lyric == null)
                throw new ArgumentNullException(nameof(lyric));

            if (algorithm == null)
                throw new InvalidOperationException($"{nameof(algorithm)} cannot be null.");

            BindableCaretPosition.Value = algorithm.CallMethod<ICaretPosition, Lyric>("MoveToTarget", lyric);
            BindableHoverCaretPosition.Value = null;
        }

        public void MoveCaretToTargetPosition(ICaretPosition position)
        {
            if (position.Lyric == null)
                return;

            if (!CaretPositionMovable(position))
                return;

            BindableHoverCaretPosition.Value = null;
            BindableCaretPosition.Value = position;
        }

        public void MoveHoverCaretToTargetPosition(ICaretPosition position)
        {
            if (position.Lyric == null)
                return;

            if (!CaretPositionMovable(position))
                return;

            BindableHoverCaretPosition.Value = position;
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
