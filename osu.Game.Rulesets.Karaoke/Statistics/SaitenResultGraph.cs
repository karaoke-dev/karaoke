// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class SaitenResultGraph : CompositeDrawable
    {
        private readonly LyricPreview lyricGraph;
        private readonly NoteGraph noteGraph;

        public SaitenResultGraph(ScoreInfo score,IBeatmap beatmap)
        {
            InternalChildren = new Drawable[]
            {
                lyricGraph = new LyricPreview(beatmap.HitObjects.OfType<LyricLine>())
                {
                    RelativeSizeAxes = Axes.Both,
                    Spacing = new Vector2(10),
                },
                noteGraph = new NoteGraph(score)
            };

            lyricGraph.SelectedLyricLine.BindValueChanged(e =>
            {
                // todo : move noteGraph to target time.
            });
        }

        internal class NoteGraph : CompositeDrawable
        {
            public NoteGraph(ScoreInfo score)
            {
                var noteEvents = score.HitEvents?.Where(x => x.HitObject is Note note && note.Display).ToList() ?? new List<HitEvent>();
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
