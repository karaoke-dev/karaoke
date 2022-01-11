// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagEditModeSection : TextTagEditModeSection
    {
        [Resolved]
        private ILyricEditorExtendAreaState lyricEditorExtendAreaState { get; set; }

        protected override TextTagEditMode DefaultMode()
            => lyricEditorExtendAreaState.RubyTagEditMode;

        protected override Dictionary<TextTagEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    TextTagEditMode.Generate, new EditModeSelectionItem("Generate", "Auto-generate rubies in the lyric.")
                },
                {
                    TextTagEditMode.Edit, new EditModeSelectionItem("Edit", "Create / delete and edit lyric rubies in here.")
                },
                {
                    TextTagEditMode.Verify, new EditModeSelectionItem("Verify", "Check invalid rubies in here.")
                }
            };

        protected override void UpdateEditMode(TextTagEditMode mode)
        {
            lyricEditorExtendAreaState.ChangeRubyTagEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }
}
