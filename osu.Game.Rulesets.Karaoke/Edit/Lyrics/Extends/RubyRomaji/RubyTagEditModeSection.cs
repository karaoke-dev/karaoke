// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RubyTagEditModeSection : TextTagEditModeSection
    {
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
    }
}
