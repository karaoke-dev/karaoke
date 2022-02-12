// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagEditModeSection : TextTagEditModeSection
    {
        [Resolved]
        private IEditRomajiModeState editRomajiModeState { get; set; }

        protected override TextTagEditMode DefaultMode()
            => editRomajiModeState.EditMode;

        protected override Dictionary<TextTagEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    TextTagEditMode.Generate, new EditModeSelectionItem("Generate", "Auto-generate romajies in the lyric.")
                },
                {
                    TextTagEditMode.Edit, new EditModeSelectionItem("Edit", "Create / delete and edit lyric romajies in here.")
                },
                {
                    TextTagEditMode.Verify, new EditModeSelectionItem("Verify", "Check invalid romajies in here.")
                }
            };

        protected override void UpdateEditMode(TextTagEditMode mode)
        {
            editRomajiModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }
}
