// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes
{
    public class EditNoteOverlay : EditOverlay
    {
        public override float ContentHeight => 180;

        public EditNoteOverlay(Lyric lyric)
            : base(lyric)
        {
        }

        protected override Drawable CreateInfo(Lyric lyric)
        {
            // todo : waiting for implementation.
            return new Container();
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            // todo : waiting for implementation.
            return new EditNoteHitObjectComposer(lyric)
            {
                RelativeSizeAxes = Axes.X,
                Height = 150,
            };
        }

        internal class EditNoteHitObjectComposer : RowEditExtendHitObjectComposer
        {
            protected Lyric TargetLyric { get; }

            public EditNoteHitObjectComposer(Lyric lyric)
            {
                TargetLyric = lyric;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                // add all matched notes into playfield
                var notes = EditorBeatmap.HitObjects.OfType<Note>().Where(x => x.ParentLyric == TargetLyric).ToList();

                foreach (var note in notes)
                {
                    Playfield.Add(note);
                }
            }

            // find time in first note.
            protected override double CurrentTime => TargetLyric.LyricStartTime;

            protected override Playfield CreatePlayfield()
                => new EditorNotePlayfield(9);

            protected override ComposeBlueprintContainer CreateBlueprintContainer()
                => new EditNoteBlueprintContainer(this);

            #region IPlacementHandler

            public override void BeginPlacement(HitObject hitObject)
            {
                throw new NotImplementedException();
            }

            public override void Delete(HitObject hitObject)
            {
                throw new NotImplementedException();
            }

            public override void EndPlacement(HitObject hitObject, bool commit)
            {
                throw new NotImplementedException();
            }

            #endregion

            internal class EditNoteBlueprintContainer : ComposeBlueprintContainer
            {
                public EditNoteBlueprintContainer(HitObjectComposer composer)
                    : base(composer)
                {
                }

                public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject)
                {
                    switch (hitObject)
                    {
                        case Note note:
                            return new NoteSelectionBlueprint(note);

                        default:
                            throw new IndexOutOfRangeException(nameof(hitObject));
                    }
                }

                protected override SelectionHandler<HitObject> CreateSelectionHandler() => new EditNoteSelectionHandler();

                internal class EditNoteSelectionHandler : KaraokeSelectionHandler
                {
                    [Resolved]
                    private HitObjectComposer composer { get; set; }

                    protected override ScrollingNotePlayfield NotePlayfield => (composer as EditNoteHitObjectComposer)?.Playfield as ScrollingNotePlayfield;
                }
            }
        }
    }
}
