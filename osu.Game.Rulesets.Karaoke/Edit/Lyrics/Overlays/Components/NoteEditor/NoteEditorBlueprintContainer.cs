// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.NoteEditor
{
    public class NoteEditorBlueprintContainer : BlueprintContainer<Note>
    {
        protected readonly Lyric Lyric;

        public NoteEditorBlueprintContainer(Lyric lyric)
        {
            Lyric = lyric;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            // todo : get notes and add in here.
        }

        protected override IEnumerable<SelectionBlueprint<Note>> SortForMovement(IReadOnlyList<SelectionBlueprint<Note>> blueprints)
            => blueprints.OrderBy(b => b.Item.StartTime);

        protected override bool ApplySnapResult(SelectionBlueprint<Note>[] blueprints, SnapResult result)
        {
            if (!base.ApplySnapResult(blueprints, result))
                return false;

            // todo : move position

            return true;
        }

        /// <summary>
        /// Commit time-tag time.
        /// </summary>
        protected override void DragOperationCompleted()
        {
            // todo : should change together.
        }

        protected override SelectionBlueprint<Note> CreateBlueprintFor(Note item)
            => new NoteEditorHitObjectBlueprint(item);

        protected override SelectionHandler<Note> CreateSelectionHandler()
            => new EditNoteSelectionHandler();

        internal class EditNoteSelectionHandler : KaraokeSelectionHandler
        {
            [Resolved]
            private HitObjectComposer composer { get; set; }

            protected override ScrollingNotePlayfield NotePlayfield => (composer as EditNoteOverlay.EditNoteHitObjectComposer)?.Playfield as ScrollingNotePlayfield;
        }
    }
}
