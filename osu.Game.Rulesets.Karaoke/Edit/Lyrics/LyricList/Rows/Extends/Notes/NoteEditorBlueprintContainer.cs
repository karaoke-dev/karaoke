// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Extends.Notes
{
    internal class EditNoteBlueprintContainer : ExtendBlueprintContainer<Note>
    {
        [UsedImplicitly]
        private readonly BindableList<Note> notes = new();

        private readonly Lyric lyric;

        public EditNoteBlueprintContainer(Lyric lyric)
        {
            this.lyric = lyric;
        }

        protected override SelectionBlueprint<Note> CreateBlueprintFor(Note hitObject)
            => new NoteEditorHitObjectBlueprint(lyric, hitObject);

        protected override SelectionHandler<Note> CreateSelectionHandler() => new NoteEditorSelectionHandler();

        [BackgroundDependencyLoader]
        private void load(IEditNoteModeState editNoteModeState, EditorBeatmap beatmap)
        {
            // todo : might deal with the cause if create or delete notes.
            notes.Clear();

            var addedNotes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);
            notes.AddRange(addedNotes);

            // Add time-tag into blueprint container
            RegisterBindable(notes);
        }

        protected class NoteEditorSelectionHandler : ExtendSelectionHandler<Note>
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
}
