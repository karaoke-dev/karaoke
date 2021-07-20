// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    internal class EditNoteBlueprintContainer : ExtendBlueprintContainer<Note>
    {
        [UsedImplicitly]
        private readonly Bindable<Note[]> notes = new Bindable<Note[]>();

        private readonly Lyric lyric;

        public EditNoteBlueprintContainer(Lyric lyric)
        {
            this.lyric = lyric;
        }

        protected override SelectionBlueprint<Note> CreateBlueprintFor(Note hitObject)
            => new NoteEditorHitObjectBlueprint(lyric, hitObject);

        protected override SelectionHandler<Note> CreateSelectionHandler() => new NoteEditorSelectionHandler();

        [BackgroundDependencyLoader]
        private void load(NoteEditor noteEditor, EditorBeatmap beatmap)
        {
            SelectedItems.BindTo(noteEditor.SelectedNotes);

            // todo : might deal with the cause if create or delete notes.
            notes.Value = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToArray();

            // Add time-tag into blueprint container
            RegisterBindable(notes);
        }

        protected class NoteEditorSelectionHandler : ExtendSelectionHandler<Note>
        {
            protected override void DeleteItems(IEnumerable<Note> items)
            {
                // todo : delete notes
                foreach (var item in items)
                {
                }
            }
        }
    }
}
