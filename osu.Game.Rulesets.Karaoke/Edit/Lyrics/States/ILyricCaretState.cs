// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States
{
    public interface ILyricCaretState
    {
        IBindable<ICaretPosition?> BindableHoverCaretPosition { get; }

        IBindable<ICaretPosition?> BindableCaretPosition { get; }

        IBindable<Lyric?> BindableFocusedLyric { get; }

        bool MoveCaret(MovingCaretAction action);

        ICaretPosition? GetCaretPositionByAction(MovingCaretAction action);

        bool MoveCaretToTargetPosition(Lyric lyric);

        bool MoveCaretToTargetPosition(ICaretPosition position);

        bool MoveHoverCaretToTargetPosition(ICaretPosition position);

        bool ConfirmHoverCaretPosition();

        bool ClearHoverCaretPosition();

        void SyncSelectedHitObjectWithCaret();

        bool CaretEnabled { get; }
    }
}
