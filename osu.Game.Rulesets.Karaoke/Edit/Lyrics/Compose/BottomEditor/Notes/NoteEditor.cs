// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Timing;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.Notes
{
    [Cached]
    public class NoteEditor : Container
    {
        private const int columns = 9;

        [Cached(typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new();

        private readonly Lyric lyric;

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
            var notes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);

            foreach (var note in notes)
            {
                Playfield.Add(note);
            }
        }

        private void addHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ReferenceLyric != lyric)
                return;

            Playfield.Add(note);
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (hitObject is not Note note)
                return;

            if (note.ReferenceLyric != lyric)
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
    }
}
