﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

public interface ILyricCaretState
{
    ICaretPosition? HoverCaretPosition => BindableHoverCaretPosition.Value;

    ICaretPosition? CaretPosition => BindableCaretPosition.Value;

    RangeCaretPosition? RangeCaretPosition => BindableRangeCaretPosition.Value;

    IBindable<ICaretPosition?> BindableHoverCaretPosition { get; }

    IBindable<ICaretPosition?> BindableCaretPosition { get; }

    IBindable<RangeCaretPosition?> BindableRangeCaretPosition { get; }

    IBindable<Lyric?> BindableFocusedLyric { get; }

    bool MoveCaret(MovingCaretAction action);

    ICaretPosition? GetCaretPositionByAction(MovingCaretAction action);

    bool MoveHoverCaretToTargetPosition(Lyric lyric);

    bool MoveHoverCaretToTargetPosition<TIndex>(Lyric lyric, TIndex index) where TIndex : notnull;

    bool ConfirmHoverCaretPosition();

    bool ClearHoverCaretPosition();

    bool MoveCaretToTargetPosition(Lyric lyric);

    bool MoveCaretToTargetPosition<TIndex>(Lyric lyric, TIndex index) where TIndex : notnull;

    bool StartDragging();

    bool MoveDraggingCaretIndex<TIndex>(TIndex index) where TIndex : notnull;

    bool EndDragging();

    void SyncSelectedHitObjectWithCaret();

    bool CaretEnabled { get; }

    bool CaretDraggable { get; }

    ICaretPositionAlgorithm? CaretPositionAlgorithm => BindableCaretPositionAlgorithm.Value;

    IBindable<ICaretPositionAlgorithm?> BindableCaretPositionAlgorithm { get; }
}
