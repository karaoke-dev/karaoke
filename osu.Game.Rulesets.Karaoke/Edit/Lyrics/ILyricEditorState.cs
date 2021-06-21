// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ILyricEditorState
    {
        #region general

        Bindable<LyricEditorMode> BindableMode { get; }

        LyricEditorMode Mode { get; set; }

        Bindable<RecordingMovingCaretMode> BindableRecordingMovingCaretMode { get; }

        BindableBool BindableAutoFocusEditLyric { get; }

        BindableInt BindableAutoFocusEditLyricSkipRows { get; }

        #endregion

        #region caret position

        Bindable<ICaretPosition> BindableHoverCaretPosition { get; }

        Bindable<ICaretPosition> BindableCaretPosition { get; }

        bool MoveCaretToTargetPosition(ICaretPosition position);

        bool MoveHoverCaretToTargetPosition(ICaretPosition position);

        void ClearHoverCaretPosition();

        bool CaretPositionMovable(ICaretPosition timeTag);

        bool CaretEnabled { get; }

        #endregion

        # region blueprint

        BindableList<TimeTag> SelectedTimeTags { get; }

        BindableList<ITextTag> SelectedTextTags { get; }

        void ClearSelectedTimeTags();

        void ClearSelectedTextTags();

        #endregion

        #region Select to apply

        BindableBool Selecting { get; }

        BindableList<Lyric> SelectedLyrics { get; }

        Action<LyricEditorSelectingAction> Action { get; }

        void StartSelecting();

        #endregion
    }
}
