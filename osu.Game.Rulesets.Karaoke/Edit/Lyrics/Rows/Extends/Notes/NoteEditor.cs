// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    [Cached]
    public class NoteEditor : Container
    {
        private const int columns = 9;

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new PreviewNotePositionInfo();

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo = new LocalScrollingInfo();

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
                    Playfield = new EditorNotePlayfield(columns)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Clock = new StopClock(lyric.LyricStartTime)
                    },
                    // layers above playfield
                    new EditNoteBlueprintContainer(lyric)
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                }
            };
        }

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;

            // add all matched notes into playfield
            var notes = beatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == lyric).ToList();

            foreach (var note in notes)
            {
                Playfield.Add(note);
            }
        }

        private void addHitObject(HitObject hitObject)
        {
            if (!(hitObject is Note note))
                return;

            if (note.ParentLyric != lyric)
                return;

            Playfield.Add(note);
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (!(hitObject is Note note))
                return;

            if (note.ParentLyric != lyric)
                return;

            Playfield.Remove(note);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (beatmap == null)
                return;

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } = new Bindable<NotePositionCalculator>(new NotePositionCalculator(columns, 12, ScrollingNotePlayfield.COLUMN_SPACING));

            public NotePositionCalculator Calculator => Position.Value;
        }

        private class LocalScrollingInfo : IScrollingInfo
        {
            public IBindable<ScrollingDirection> Direction { get; } = new Bindable<ScrollingDirection>(ScrollingDirection.Left);

            public IBindable<double> TimeRange { get; } = new BindableDouble(5000)
            {
                MinValue = 1000,
                MaxValue = 10000
            };

            public IScrollAlgorithm Algorithm { get; } = new SequentialScrollAlgorithm(new List<MultiplierControlPoint>());
        }
    }
}
