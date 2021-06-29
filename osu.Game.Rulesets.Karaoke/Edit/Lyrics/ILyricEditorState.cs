// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

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

        # region blueprint

        BindableList<TimeTag> SelectedTimeTags { get; }

        BindableList<RubyTag> SelectedRubyTags { get; }

        BindableList<RomajiTag> SelectedRomajiTags { get; }

        void ClearSelectedTimeTags();

        void ClearSelectedTextTags();

        #endregion
    }
}
