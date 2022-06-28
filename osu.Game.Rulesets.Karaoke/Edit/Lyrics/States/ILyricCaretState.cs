// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface ILyricCaretState
    {
        IBindable<ICaretPosition?> BindableHoverCaretPosition { get; }

        IBindable<ICaretPosition?> BindableCaretPosition { get; }

        IBindable<ICaretPositionAlgorithm?> BindableCaretPositionAlgorithm { get; }

        bool MoveCaret(MovingCaretAction action);

        void MoveCaretToTargetPosition(Lyric lyric);

        void MoveCaretToTargetPosition(ICaretPosition position);

        void MoveHoverCaretToTargetPosition(ICaretPosition position);

        void ClearHoverCaretPosition();

        bool CaretPositionMovable(ICaretPosition position);

        void SyncSelectedHitObjectWithCaret();

        bool CaretEnabled { get; }
    }
}
