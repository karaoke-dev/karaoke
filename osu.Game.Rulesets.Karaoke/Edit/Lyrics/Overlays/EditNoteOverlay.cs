// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays
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

        internal class EditNoteHitObjectComposer : OverlayHitObjectComposer
        {
            protected Lyric TargetLyric { get; private set; }

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
                    // todo : should support pooling.
                    var drawableNote = new DrawableNote(note);
                    Playfield.Add(drawableNote);
                }
            }

            protected override Playfield CreatePlayfield()
                => new NotePlayfield(9);

            #region IPlacementHandler

            public override void BeginPlacement(HitObject hitObject)
            {
                throw new System.NotImplementedException();
            }

            public override void Delete(HitObject hitObject)
            {
                throw new System.NotImplementedException();
            }

            public override void EndPlacement(HitObject hitObject, bool commit)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }
    }
}
