// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.BottomEditor.Notes;

internal partial class EditNoteBlueprintContainer : BindableBlueprintContainer<Note>
{
    protected override SelectionBlueprint<Note> CreateBlueprintFor(Note hitObject)
        => new NoteEditorSelectionBlueprint(hitObject);

    protected override SelectionHandler<Note> CreateSelectionHandler() => new NoteEditorSelectionHandler();

    protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<Note> blueprint, Vector2[] originalSnapPositions)> blueprints)
    {
        // todo: implement able to drag to change the tone.
        return false;
    }

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
