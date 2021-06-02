// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditor
    {
        private Dictionary<LyricEditorMode, ICaretPositionAlgorithm> caretMovingAlgorithmSet = new Dictionary<LyricEditorMode, ICaretPositionAlgorithm>();

        private void createAlgorithmList()
        {
            var lyrics = BindableLyrics.ToArray();
            caretMovingAlgorithmSet = new Dictionary<LyricEditorMode, ICaretPositionAlgorithm>
            {
                { LyricEditorMode.Manage, new CuttingCaretPositionAlgorithm(lyrics) },
                { LyricEditorMode.Typing, new TypingCaretPositionAlgorithm(lyrics) },
                { LyricEditorMode.EditRubyRomaji, new NavigateCaretPositionAlgorithm(lyrics) },
                { LyricEditorMode.EditNote, new NavigateCaretPositionAlgorithm(lyrics) },
                { LyricEditorMode.RecordTimeTag, new TimeTagCaretPositionAlgorithm(lyrics) { Mode = RecordingMovingCaretMode } },
                { LyricEditorMode.EditTimeTag, new TimeTagIndexCaretPositionAlgorithm(lyrics) },
                { LyricEditorMode.Layout, new NavigateCaretPositionAlgorithm(lyrics) },
                { LyricEditorMode.Singer, new NavigateCaretPositionAlgorithm(lyrics) },
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

        public void ResetPosition(LyricEditorMode mode)
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
}
