// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface ILyricCaretState
    {
        Bindable<ICaretPosition> BindableHoverCaretPosition { get; }

        Bindable<ICaretPosition> BindableCaretPosition { get; }

        void ChangePositionAlgorithm(LyricEditorMode lyricEditorMode, MovingTimeTagCaretMode movingTimeTagCaretMode);

        bool MoveCaret(MovingCaretAction action);

        void MoveCaretToTargetPosition(Lyric lyric);

        void MoveCaretToTargetPosition(ICaretPosition position);

        void MoveHoverCaretToTargetPosition(ICaretPosition position);

        void ClearHoverCaretPosition();

        void ResetPosition(LyricEditorMode mode);

        bool CaretPositionMovable(ICaretPosition position);

        bool CaretEnabled { get; }
    }
}
