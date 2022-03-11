// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class LyricEditorEditModeSection : EditModeSection<LyricEditorMode>
    {
        [Resolved]
        private ILyricEditorState state { get; set; }

        protected sealed override LyricEditorMode DefaultMode()
            => state.Mode;

        protected override void UpdateEditMode(LyricEditorMode mode)
        {
            // update mode back to lyric editor.
            state.SwitchMode(mode);

            base.UpdateEditMode(mode);
        }
    }
}
