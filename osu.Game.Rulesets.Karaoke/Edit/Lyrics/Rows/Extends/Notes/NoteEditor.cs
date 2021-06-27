// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    [Cached]
    public class NoteEditor : Container
    {
        private const int row_amount = 9;

        private readonly Lyric lyric;

        public BindableList<Note> SelectedNotes { get; } = new BindableList<Note>();

        public EditorNotePlayfield Playfield { get; }

        public NoteEditor(Lyric lyric)
        {
            this.lyric = lyric;
            InternalChild = new Container
            {
                Name = "Content",
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    // layers below playfield
                    Playfield = new EditorNotePlayfield(row_amount),
                    // layers above playfield
                    new EditNoteBlueprintContainer(lyric),
                }
            };

            // set stop clock and navigation to target time.
            Playfield.Clock = new StopClock(lyric.LyricStartTime);
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            // add all matched notes into playfield
            var notes = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToList();

            foreach (var note in notes)
            {
                Playfield.Add(note);
            }
        }
    }
}
