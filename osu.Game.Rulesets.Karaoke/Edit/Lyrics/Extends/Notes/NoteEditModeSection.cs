// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes
{
    public class NoteEditModeSection : EditModeSection<IEditNoteModeState, NoteEditMode>
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Blue;

        protected override EditModeSelectionItem CreateSelectionItem(NoteEditMode editMode) =>
            editMode switch
            {
                NoteEditMode.Generate => new EditModeSelectionItem("Generate", "Using time-tag to create default notes."),
                NoteEditMode.Edit => new EditModeSelectionItem("Edit", "Batch edit note property in here."),
                NoteEditMode.Verify => new EditModeSelectionItem("Verify", "Check invalid notes in here."),
                _ => throw new ArgumentOutOfRangeException(nameof(editMode), editMode, null)
            };

        protected override Color4 GetColour(OsuColour colours, NoteEditMode mode, bool active) =>
            mode switch
            {
                NoteEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
                NoteEditMode.Edit => active ? colours.Red : colours.RedDarker,
                NoteEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
    }
}
