// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.Notes
{
    [Cached]
    public class NoteEditor : Container
    {
        private const int columns = 9;

        [Cached(typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new();

        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        private readonly IBindable<Lyric?> bindableFocusedLyric = new Bindable<Lyric?>();

        [Cached]
        private readonly BindableList<Note> bindableNotes = new();

        [Cached(typeof(Playfield))]
        public EditorNotePlayfield Playfield { get; }

        public NoteEditor()
        {
            InternalChild = new Container
            {
                Name = "Content",
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    // layers below playfield
                    Playfield = new EditorNotePlayfield(columns)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                    // layers above playfield
                    new EditNoteBlueprintContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                }
            };

            bindableFocusedLyric.BindValueChanged(e =>
            {
                bindableNotes.Clear();

                var lyric = e.NewValue;
                if (lyric == null)
                    return;

                Playfield.Clock = new StopClock(lyric.LyricStartTime);

                // add all matched notes into playfield
                var notes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);
                bindableNotes.AddRange(notes);
            });

            bindableNotes.BindCollectionChanged((_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var obj in args.NewItems.OfType<Note>())
                            Playfield.Add(obj);

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var obj in args.OldItems.OfType<Note>())
                            Playfield.Remove(obj);

                        break;
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(ILyricCaretState lyricCaretState)
        {
            bindableFocusedLyric.BindTo(lyricCaretState.BindableFocusedLyric);

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;
        }

        private void addHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ReferenceLyric != bindableFocusedLyric.Value)
                return;

            bindableNotes.Add(note);
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ReferenceLyric != bindableFocusedLyric.Value)
                return;

            bindableNotes.Remove(note);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } = new Bindable<NotePositionCalculator>(new NotePositionCalculator(columns, 12, ScrollingNotePlayfield.COLUMN_SPACING));

            public NotePositionCalculator Calculator => Position.Value;
        }
    }
}
