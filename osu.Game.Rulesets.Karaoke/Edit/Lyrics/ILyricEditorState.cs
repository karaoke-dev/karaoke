// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ILyricEditorState
    {
        #region general

        Bindable<Mode> BindableMode { get; }

        Mode Mode { get; }

        Bindable<LyricFastEditMode> BindableFastEditMode { get; }

        Bindable<RecordingMovingCursorMode> BindableRecordingMovingCursorMode { get; }

        BindableBool BindableAutoFocusEditLyric { get; }

        BindableInt BindableAutoFocusEditLyricSkipRows { get; }

        #endregion

        #region cursor position

        Bindable<CursorPosition> BindableHoverCursorPosition { get; }

        Bindable<CursorPosition> BindableCursorPosition { get; }

        bool MoveCursorToTargetPosition(CursorPosition position);

        bool MoveHoverCursorToTargetPosition(CursorPosition position);

        void ClearHoverCursorPosition();

        bool CursorMovable(CursorPosition timeTag);

        #endregion
    }
}
