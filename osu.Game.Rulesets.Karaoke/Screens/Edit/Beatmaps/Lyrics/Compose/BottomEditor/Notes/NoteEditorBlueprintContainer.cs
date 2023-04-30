// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.BottomEditor.Notes;

internal partial class EditNoteBlueprintContainer : BindableBlueprintContainer<Note>
{
    protected override SelectionBlueprint<Note> CreateBlueprintFor(Note hitObject)
        => new NoteEditorSelectionBlueprint(hitObject);

    protected override SelectionHandler<Note> CreateSelectionHandler() => new NoteEditorSelectionHandler();

    [BackgroundDependencyLoader]
    private void load(BindableList<Note> notes)
    {
        // Add time-tag into blueprint container
        RegisterBindable(notes);
    }

    protected partial class NoteEditorSelectionHandler : BindableSelectionHandler
    {
        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState)
        {
            SelectedItems.BindTo(editNoteModeState.SelectedItems);
        }

        protected override void DeleteItems(IEnumerable<Note> items)
        {
            // todo : delete notes
            foreach (var item in items)
            {
            }
        }
    }
}
