// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class SaitenResultGraph : CompositeDrawable
    {
        private readonly LyricGraph lyricGraph;
        private readonly NoteGraph noteGraph;

        public SaitenResultGraph(ScoreInfo score)
        {
            InternalChild = new OsuScrollContainer
            {
                Children = new Drawable[]
                {
                    lyricGraph = new LyricGraph(score),
                    noteGraph = new NoteGraph(score)
                }
            };
        }

        internal class LyricGraph : CompositeDrawable
        {
            public LyricGraph(ScoreInfo score)
            {
                // TODO : convert into lyric.
                var events = score.HitEvents.ToList();
                foreach (var e in events)
                {
                    // TOOD : create path
                }
            }
        }

        internal class NoteGraph : CompositeDrawable
        {
            public NoteGraph(ScoreInfo score)
            {
                var noteEvents = score.HitEvents.Where(x => x.HitObject is Note note && note.Display).ToList();
                foreach (var noteEvent in noteEvents)
                {
                    // TOOD : add note into here
                }

                // todo : add list of note colors to present state.
            }

            internal class DrawableNote : Box
            {
                internal DrawableNote(HitResult result)
                {
                    // TOOD : assign color with different hit result.
                }
            }
        }
    }
}
