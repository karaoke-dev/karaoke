// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditModeSection : LyricEditorEditModeSection
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Blue;

        protected override Dictionary<LyricEditorMode, EditModeSelectionItem> CreateSelections()
            => new Dictionary<LyricEditorMode, EditModeSelectionItem>
            {
                {
                    LyricEditorMode.CreateNote, new EditModeSelectionItem("Create", "Using time-tag to create default notes.")
                },
                {
                    LyricEditorMode.CreateNotePosition, new EditModeSelectionItem("Position", "Using singer voice data to adjust note position.")
                },
                {
                    LyricEditorMode.AdjustNote, new EditModeSelectionItem("Adjust", "If you are note satisfied this result, you can adjust this by hands.")
                }
            };

        protected override Color4 GetColour(OsuColour colour, LyricEditorMode mode, bool active) =>
            mode switch
            {
                LyricEditorMode.CreateNote => active ? colour.Blue : colour.BlueDarker,
                LyricEditorMode.CreateNotePosition => active ? colour.Red : colour.RedDarker,
                LyricEditorMode.AdjustNote => active ? colour.Yellow : colour.YellowDarker,
                _ => throw new IndexOutOfRangeException(nameof(mode))
            };
    }
}
