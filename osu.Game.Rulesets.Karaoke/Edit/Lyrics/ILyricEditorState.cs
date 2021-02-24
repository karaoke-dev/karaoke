// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ILyricEditorState
    {
        #region general

        Bindable<Mode> BindableMode { get; }

        Mode Mode { get; }

        Bindable<LyricFastEditMode> BindableFastEditMode { get; }

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

        bool CaretMovable(ICaretPosition timeTag);

        #endregion
    }
}
