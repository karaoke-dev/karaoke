// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ILyricEditorState
    {
        Bindable<LyricEditorMode> BindableMode { get; }

        LyricEditorMode Mode { get; set; }

        Bindable<RecordingMovingCaretMode> BindableRecordingMovingCaretMode { get; }

        void NavigateToFix(LyricEditorMode mode);
    }
}
